using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Admin.PartialViews
{
    public class _PizzasTableModel : PageModel
    {
        private readonly ApplicationDB _context;

        public string[] Ingedients => Helper.GetPossibleIngredients();
        public List<PizzaDBModel> Pizzas { get; set; } = new List<PizzaDBModel>();

        public _PizzasTableModel(ApplicationDB context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var sessionId = this.Request.Cookies["sessionId"];

            if (Sessions.CheckSessionId(sessionId))
            {
                Pizzas = await GetPizzas();
                return Page();
            }
            else
            {
                this.Response.StatusCode = 401;
                return Redirect("/Index");
            }
        }
        private async Task<List<PizzaDBModel>> GetPizzas()
        {
            return await _context.Pizzas.AsNoTracking().ToListAsync();
        }

    }
}
