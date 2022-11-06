using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;
using System.Text.Json;

namespace Online_Pizzeria.Pages.Forms
{
    public class CustomPizzaModel : PageModel
    {
        [BindProperty]
        public OrderUserModel UserOrder { get; set; }

        private readonly ApplicationDB _context;
        private readonly IConfiguration _configuration;
        private readonly Mapper<OrderUserModel, OrderDBModel> _mapper;
        private readonly decimal _defaultPrice;
        private readonly decimal _defaultIngredientPrice;

        public CustomPizzaModel(ApplicationDB context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mapper = new Mapper<OrderUserModel, OrderDBModel>();
            _defaultPrice = GetDefaultPrice();
            _defaultIngredientPrice = GetDefaultIngredientPrice();
        }

        public void OnGet()
        {           
        }

        public IActionResult OnPost()
        {
            var ingredients = ParseIngredients();
            UserOrder.UserPizza.Name ??= "Custom Pizza";
            UserOrder.UserPizza.BasePrice = _defaultPrice;
            UserOrder.Price = UserOrder.UserPizza.BasePrice + _defaultIngredientPrice * ingredients.Count;
            UserOrder.Ordered = DateTime.Now;

            var UserOrderDB = _mapper.Map(UserOrder);

            if (UserOrderDB != null)
            {
                _context.PizzaOrders.Add(UserOrderDB);
                _context.SaveChanges();

                var OrderId = _context.PizzaOrders.OrderBy(p => p.Id).Last().Id;

                return Redirect($"/Checkout/Checkout?OrderId={OrderId}");
            }
            else throw new ArgumentNullException("There is a error while mapping UserOrder");
        }

        private List<Ingredient> ParseIngredients()
        {
            var list = new List<Ingredient>();
            if (UserOrder.UserPizza.Ingredients is null)
            {
                return list;
            }

            foreach (var IngredientName in Enum.GetNames(typeof(Ingredient)))
            {
                if (UserOrder.UserPizza.Ingredients.Contains(IngredientName))
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
