using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;
using System.IO;

namespace Online_Pizzeria.Pages.Forms
{
    public class PizzaModel : PageModel
    {
        public List<PizzaUserModel> pizzasUser;

        private readonly ApplicationDB _context;

        public PizzaModel(ApplicationDB context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var pizzasDB = _context.Pizzas.ToArray() ?? throw new ArgumentNullException();

            pizzasUser = new List<PizzaUserModel>();
            foreach (var pizza in pizzasDB)
            {
                pizzasUser.Add(new Mapper<PizzaDBModel, PizzaUserModel>().Map(pizza));
            }

            return Page();
        }
    }
}
