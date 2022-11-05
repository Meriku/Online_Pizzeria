using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Checkout
{
    public class CheckoutModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PizzasModel Order { get; set; }

        private readonly ApplicationDB _context;
        public CheckoutModel(ApplicationDB context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var OrderToDB = Order as PizzaDBModel;

            _context.PizzaOrders.Add(OrderToDB);
            _context.SaveChanges();

        }
    }
}
