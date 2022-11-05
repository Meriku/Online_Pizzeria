using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Checkout
{
    public class CheckoutModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PizzasModel Order { get; set; }

        public void OnGet()
        {
        }
    }
}
