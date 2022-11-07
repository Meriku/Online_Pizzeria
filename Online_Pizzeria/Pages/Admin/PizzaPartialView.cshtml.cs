using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Admin
{
    public class PizzaPartialViewModel : PageModel
    {
        public PizzaDBModel Pizza { get; set; }
        public void OnGet()
        {
        }
    }
}
