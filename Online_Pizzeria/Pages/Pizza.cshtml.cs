using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Online_Pizzeria.Pages.Forms
{
    public class PizzaModel : PageModel
    {
        public string[] PizzasNames = { "Bolognese", "Carbonara", "Hawaiian", "Margerita", "MeatFeast", "Mushroom", "Pepperoni", "Seafood", "Vegetarian" };
        public void OnGet()
        {
        }
    }
}
