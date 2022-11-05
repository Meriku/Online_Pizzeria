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

        public async Task OnGet()
        {
            await _context.PizzaOrders.AddAsync(Order as PizzaDBModel);
            _context.SaveChanges();
        }

    }
}
