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
        private Mapper<OrderUserModel, OrderDBModel> _mapper;
        public OrderDBModel UserOrder { get; set; }

        public CheckoutModel(ApplicationDB context)
        {
            _context = context;
            _mapper = new Mapper<OrderUserModel, OrderDBModel>();
        }

        public void OnGet()
        {
            var CustomPizzaOrderId = this.Request.Query["OrderId"];
            var PizzaFromDBName = this.Request.Query["PizzaName"];

            if (Helper.ParseInt(CustomPizzaOrderId, out int id)) 
            { 
                UserOrder = _context.PizzaOrders.First(x => x.Id == id); 
            }

            else if (PizzaFromDBName.Count > 0) 
            {
                var pizza = _context.Pizzas.FirstOrDefault(x => x.Name.Equals(PizzaFromDBName[0]));

                if (pizza == null)
                {
                    throw new ArgumentNullException("Pizza null on Checkout");
                }

                UserOrder = new OrderDBModel() 
                { 
                    PizzaId = pizza.Id,
                    Price = pizza.BasePrice,
                    StatusCode = Models.StatusCodes.Ordered,
                    Ordered = DateTime.Now
                };

                _context.PizzaOrders.Add(UserOrder);
                _context.SaveChanges();

            } 
            
        }

    }
}
