using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Forms
{
    public class CustomPizzaModel : PageModel
    {
        [BindProperty]
        public PizzasUserModel Pizza { get; set; }
        private decimal? _defaultPizzaPrice;
        private decimal? _defaultIngredientPrice;


        private readonly IConfiguration _configuration;
        public CustomPizzaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {           
        }

        public IActionResult OnPost()
        {
            _defaultPizzaPrice = _defaultPizzaPrice ?? GetDefaultPrice();
            _defaultIngredientPrice = _defaultIngredientPrice ?? GetDefaultIngredientPrice();
            var Ingredients = ParseIngredients();

            var Order = new Online_Pizzeria.Models.PizzasModel
            {
                Name = string.IsNullOrWhiteSpace(Pizza.Name) ? "Not Specified" : Pizza.Name,
                Ingredients = Ingredients,
                ImageTitle = "Create",
                Price = (decimal)_defaultPizzaPrice + decimal.Multiply((decimal)_defaultIngredientPrice, Ingredients.Count())
            };

            return RedirectToPage("/Checkout/Checkout", Order);
        }

        private List<Ingredient> ParseIngredients()
        {     
            var list = new List<Ingredient>();
            if (Pizza.Ingredients is null)
            {
                return list;
            }

            foreach (var IngredientName in Enum.GetNames(typeof(Ingredient)))
            {
                if (Pizza.Ingredients.Contains(IngredientName))
                {
                    list.Add(Enum.Parse<Ingredient>(IngredientName));
                }
            }
            return list;
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

        private decimal GetDefaultIngredientPrice()
        {
            var price = _configuration.GetValue<string>("IngredientPrice");
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
