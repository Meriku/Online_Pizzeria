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

        public AdminModel(ApplicationDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            var deletePizzaId = this.Request.Query["deletePizzaId"];

            if (Sessions.CheckSessionId(this.Request.Cookies["sessionId"]))
            {
                if (Helper.ParseInt(deletePizzaId, out int Id)) { DeletePizza(Id); }

                return Page();
            }
            else
            {
                this.Response.StatusCode = 401;
                return RedirectToPage("/Index");
            }
        }

        public void OnPost()
        {
            if (Sessions.CheckSessionId(this.Request.Cookies["sessionId"]))
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
                if (requestName.Equals("EditPizzaRequest"))
                {
                    EditPizza();
                }
            }
            else
            {
                this.Response.StatusCode = 401;
                RedirectToPage("/Index");
            }        
        }

        private void CreatePizza()
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
        }
        private void EditPizza()
        {
            var editPizzaRequest = new EditPizzaRequest()
            {
                PizzaId = this.Request.Headers["PizzaId"][0],
                Name = this.Request.Headers["Name"][0],
                BasePrice = this.Request.Headers["BasePrice"][0],
                Ingredients = this.Request.Headers["Ingredients"][0]
            };

            if (Helper.ParseInt(editPizzaRequest.PizzaId, out int id))
            {
                var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);
                if (pizza == null)
                {
                    throw new ArgumentNullException("Pizza was null while editing");
                }

                //TODO: refactoring
                pizza.Name = String.IsNullOrWhiteSpace(editPizzaRequest.Name) ? pizza.Name : editPizzaRequest.Name;
                pizza.BasePrice = Helper.ParseDecimal(editPizzaRequest.BasePrice, out decimal price) ? price : pizza.BasePrice;
                pizza.Ingredients = String.IsNullOrWhiteSpace(editPizzaRequest.Ingredients) ? pizza.Ingredients : editPizzaRequest.Ingredients;

                _context.SaveChanges();
            }
        }
        private void EditOrder()
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
