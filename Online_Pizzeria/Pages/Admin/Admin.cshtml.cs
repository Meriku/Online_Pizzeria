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
        
        public AdminModel(ApplicationDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            if (Sessions.CheckSessionId(this.Request.Cookies["sessionId"]))
            {
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
                switch (this.Request.Headers["RequestName"][0])
                {
                    case "EditOrderRequest":
                        EditOrder();
                        break;
                    case "CreatePizzaRequest":
                        CreatePizza();
                        break;
                    case "EditPizzaRequest":
                        EditPizza();
                        break;
                    case "DeletePizzaRequest":
                        DeletePizza();
                        break;
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

        private void DeletePizza()
        {
            var deletePizzaRequest = new DeletePizzaRequest()
            {
                PizzaId = this.Request.Headers["PizzaId"][0]
            };

            if (Helper.ParseInt(deletePizzaRequest.PizzaId, out int id))
            {
                _context.Pizzas.Remove(new PizzaDBModel() { Id = id });
                _context.SaveChanges();
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
