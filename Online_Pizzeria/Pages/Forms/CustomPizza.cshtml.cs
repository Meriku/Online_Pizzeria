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
        public string[] Ingedients => Helper.GetPossibleIngredients();

        public CustomPizzaModel()
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
