using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;
using Online_Pizzeria.Models.Requests;

namespace Online_Pizzeria.Pages.Admin
{
    public class AdminModel : PageModel
    {
        private readonly ApplicationDB _context;
        private readonly IConfiguration _configuration;
        
        public string[] Ingedients => Helper.GetPossibleIngredients();
        public string[] Statuses => Helper.GetPossibleStatuses();

        public bool IsOrders { get; set; }

        public AdminModel(ApplicationDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            IsOrders = this.Request.Query["view"].Equals("Orders");
            var deletePizzaId = this.Request.Query["deletePizzaId"];

            var sessionId = this.Request.Cookies["sessionId"];
            if (Sessions.CheckSessionId(sessionId))
            {
                if (Helper.ParseInt(deletePizzaId, out int Id)) { DeletePizza(Id); }

                return Page();
            }
            else
            {
                this.Response.StatusCode = 401;
                return Redirect("/Index");
            }
        }

        public void OnPost()
        {
            var sessionId = this.Request.Cookies["sessionId"];
            
            if (Sessions.CheckSessionId(sessionId))
            {
                var requestName = this.Request.Headers["RequestName"][0];

                if (requestName.Equals("EditOrderRequest"))
                {
                    EditOrder();
                }
                if (requestName.Equals("CreatePizzaRequest"))
                {
                    CreatePizza();
                }
            }
            else
            {
                this.Response.StatusCode = 401;
                RedirectToPage("/Index");
            }        
        }

        private IActionResult CreatePizza()
        {
            var createPizzaRequest = new CreatePizzaRequest()
            {
                Name = this.Request.Headers["Name"][0],
                BasePrice = this.Request.Headers["BasePrice"][0],
                Ingredients = this.Request.Headers["Ingredients"][0]
            };

            var pizza = new PizzaDBModel()
            {
                Name = createPizzaRequest.Name ?? "Default Name",
                BasePrice = Helper.ParseDecimal(createPizzaRequest.BasePrice, out decimal pri) ? pri : GetDefaultPrice(),
                Ingredients = createPizzaRequest.Ingredients
            };

            _context.Pizzas.Add(pizza);
            _context.SaveChanges();

            return Page();
        }

        private IActionResult EditOrder()
        {
            var editOrderRequest = new EditOrderRequest()
            {
                OrderId = this.Request.Headers["OrderId"][0],
                OrderStatus = this.Request.Headers["OrderStatus"][0]
            };

            if (Helper.ParseInt(editOrderRequest.OrderId, out int id))
            {
                var order = _context.PizzaOrders.FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    throw new ArgumentNullException("Order was null while editing");
                }
                var statusCode = Helper.GetEnum(editOrderRequest.OrderStatus);
                order.StatusCode = statusCode;

                if (statusCode == Models.StatusCodes.Delivered)
                {
                    order.Delivered = DateTime.Now;
                }
                _context.SaveChanges();
            }
            return RedirectToPage("/Admin/Admin");
        }

        private void DeletePizza(int? id)
        {
            if (id == null) { throw new ArgumentNullException("Pizza id was null"); }
            try
            {
                _context.Pizzas.Remove(new PizzaDBModel() { Id = (int)id });
                _context.SaveChanges();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private decimal GetDefaultPrice()
        {
            var price = _configuration.GetValue<string>("DefaultPrice");
            try
            {
                var priceDecimal = Decimal.Parse(price);
                return priceDecimal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
