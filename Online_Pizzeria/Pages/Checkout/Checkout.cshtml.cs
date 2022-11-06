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

        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }

        public CheckoutModel(ApplicationDB context)
        {
            _context = context;
            _mapper = new Mapper<OrderUserModel, OrderDBModel>();
        }

        public void OnGet()
        {
            UserOrder = _context.PizzaOrders.First(x => x.Id == OrderId);
        }

    }
}
