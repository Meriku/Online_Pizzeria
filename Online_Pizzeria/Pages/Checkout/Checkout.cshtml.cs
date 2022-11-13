using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;
using System.Text.Json;

namespace Online_Pizzeria.Pages.Checkout
{
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDB _context;
        private readonly IConfiguration _configuration;

        public PizzaUserModel UserPizza;

        public CheckoutModel(ApplicationDB context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
        }

        public IActionResult OnGet([FromQuery] string pizzaName, [FromQuery] string pizzaIngredients)
        {
            if (!string.IsNullOrWhiteSpace(pizzaName) && string.IsNullOrWhiteSpace(pizzaIngredients))
            {
                var pizzaDB = _context.Pizzas.FirstOrDefault(x => x.Name.Equals(pizzaName));

                if (pizzaDB == null) { return RedirectToPage("/Pizza"); }

                UserPizza = new Mapper<PizzaDBModel, PizzaUserModel>().Map(pizzaDB);

                return Page();
            }
            else if (!string.IsNullOrWhiteSpace(pizzaName) && !string.IsNullOrWhiteSpace(pizzaIngredients) && pizzaIngredients.Length > 3)
            {
                var ingredientsList = ParseIngredients(pizzaIngredients);
                var price = _configuration.GetDefaultPrice() + _configuration.GetDefaultIngredientPrice() * ingredientsList.Count;

                UserPizza = new PizzaUserModel()
                {
                    Name = pizzaName.Equals("Custom Pizza") ? pizzaName : "CustomPizza_" + pizzaName,
                    ImageName = "Create",
                    Ingredients = ingredientsList.ToIngredientsString(),
                    BasePrice = price
                };

                return Page();
            }
            else
            {
                Response.StatusCode = 404;
                return RedirectToPage("/Pizza");
            }

        }

        public IActionResult OnPost()
        {
            switch (this.Request.Headers["RequestName"][0])
            {
                case "SendNewOrderRequest":
                    return SendNewOrderRequest();

                default:
                    Response.StatusCode = 404;
                    return RedirectToPage("/Index");
            }

        }

        private IActionResult SendNewOrderRequest()
        {
            var pizzaName = this.Request.Headers["PizzaName"][0];
            var pizzaIngredients = this.Request.Headers["Ingredients"][0];

            var ingredientsList = ParseIngredients(pizzaIngredients);
            var priceCustom = _configuration.GetDefaultPrice() + _configuration.GetDefaultIngredientPrice() * ingredientsList.Count;


            if (string.IsNullOrWhiteSpace(pizzaName)) 
            {
                Response.StatusCode = 404;
                return RedirectToPage("/Index");
            };

            var pizza = _context.Pizzas.FirstOrDefault(p => p.Name.Equals(pizzaName));
            if (pizza != null)
            {
                var order = new OrderDBModel()
                {
                    PizzaId = pizza.Id,
                    Price = pizza.BasePrice,
                    StatusCode = Models.StatusCodes.Ordered,
                    Ordered = DateTime.Now
                };

                _context.PizzaOrders.Add(order);
                _context.SaveChanges();

                Response.StatusCode = 200;
                return RedirectToPage("/Checkout/ThankYou");
            }
            else if (!string.IsNullOrWhiteSpace(pizzaName) && !string.IsNullOrWhiteSpace(pizzaIngredients))
            {
                var order = new OrderDBModel()
                {
                    Price = priceCustom,
                    StatusCode = Models.StatusCodes.Ordered,
                    Ordered = DateTime.Now
                };
                _context.PizzaOrders.Add(order);
                _context.SaveChanges();

                Response.StatusCode = 200;
                return RedirectToPage("/Checkout/ThankYou");
            }
            else
            {
                Response.StatusCode = 404;
                return RedirectToPage("/Index");
            }

            Response.StatusCode = 404;
            return RedirectToPage("/Index");
        }

        private List<Ingredient> ParseIngredients(string ingredients)
        {
            var list = new List<Ingredient>();

            foreach (var IngredientName in Helper.GetPossibleIngredients())
            {
                if (ingredients.Contains(IngredientName))
                {
                    list.Add(Enum.Parse<Ingredient>(IngredientName));
                }
            }

            return list;
        }

    }
}
