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
            else if (!string.IsNullOrWhiteSpace(pizzaName) && !string.IsNullOrWhiteSpace(pizzaIngredients))
            {
                var ingredientsList = ParseIngredients(pizzaIngredients);
                var price = _configuration.GetDefaultPrice() + _configuration.GetDefaultIngredientPrice() * ingredientsList.Count;

                UserPizza = new PizzaUserModel()
                {
                    Name = pizzaName,
                    ImageName = "Create",
                    Ingredients = ingredientsList.ToIngredientsString(),
                    BasePrice = price
                };

                return Page();
            }
            else
            {
                Response.StatusCode = 404;
                return RedirectToPage("/Index");
            }

        }

        public void OnPost()
        {
            switch (this.Request.Headers["RequestName"][0])
            {
                case "SendNewOrderRequest":
                    SendNewOrderRequest();
                    break;

                default:
                    Response.StatusCode = 404;
                    RedirectToPage("/Index");
                    break;
            }

        }

        private IActionResult SendNewOrderRequest()
        {
            

            ////TODO


            Response.StatusCode = 200;
            return Page();
        }

        private List<Ingredient> ParseIngredients(string ingredients)
        {
            var list = new List<Ingredient>();
            var ingredientsArray = Array.Empty<string>();

            try
            {
                ingredientsArray = JsonSerializer.Deserialize<string[]>(ingredients);
            }
            catch
            {
                return list;
            }

            foreach (var IngredientName in Helper.GetPossibleIngredients())
            {
                if (ingredientsArray.Contains(IngredientName))
                {
                    list.Add(Enum.Parse<Ingredient>(IngredientName));
                }
            }

            return list;
        }

    }
}
