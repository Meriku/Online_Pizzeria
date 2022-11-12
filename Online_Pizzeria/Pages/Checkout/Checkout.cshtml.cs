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
        private readonly Mapper<PizzaDBModel, PizzaUserModel> _mapper = new();
        public OrderUserModel UserOrder { get; set; }

        public CheckoutModel(ApplicationDB context)
        {
            _context = context;
        }

        public void OnGet([FromQuery] string pizzaName, [FromQuery] string pizzaIngredients)
        {
            if (pizzaName.Count() > 0) //TODO
            {
                var pizza = _context.Pizzas.FirstOrDefault(x => x.Name.Equals(pizzaName));
                
                if (pizza == null)
                {
                    throw new ArgumentNullException("Pizza null on Checkout");
                }

                UserOrder = new OrderUserModel()
                {
                    UserPizza = _mapper.Map(pizza),
                    Price = pizza.BasePrice,
                    Ordered = DateTime.Now
                };
            }
            
        }

        public IActionResult OnPost()
        {
            if (Sessions.CheckSessionId(this.Request.Cookies["sessionId"]))
            {
                switch (this.Request.Headers["RequestName"][0])
                {
                    case "SendNewOrderRequest":
                        return SendNewOrderRequest();
                }
                Response.StatusCode = 404;
                return RedirectToPage("/Index");
            }
            else
            {
                Response.StatusCode = 401;
                return RedirectToPage("/Index");
            }
        }

        private IActionResult SendNewOrderRequest()
        {
            var UserOrderDB = new Mapper<OrderUserModel, OrderDBModel>().Map(UserOrder);

            UserOrderDB.StatusCode = Models.StatusCodes.Ordered;
            UserOrderDB.PizzaId = _context.Pizzas.FirstOrDefault(x => x.Name.Equals(UserOrder.UserPizza.Name)).Id;

            //TODO

            _context.PizzaOrders.Add(UserOrderDB);
            _context.SaveChanges();

            Response.StatusCode = 200;
            return Page();
        }

    }
}
