using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Admin
{
    public class AdminModel : PageModel
    {

        private readonly ApplicationDB _context;
        public bool IsOrders { get; set; }
        //public DbSet<OrderDBModel> PizzaOrders { get; set; }
        //public DbSet<PizzaDBModel> Pizzas { get; set; }
        public List<OrderDBModel> PizzaOrders { get; set;}
        public List<PizzaDBModel> Pizzas { get; set; }

        public AdminModel(ApplicationDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionId = this.Request.Cookies["sessionId"];
            IsOrders = this.Request.Query["view"].Equals("Orders");

            if (Sessions.CheckSessionId(sessionId))
            {
                PizzaOrders = await GetOrders();
                Pizzas = await GetPizzas();

                return Page();
            }
            else
            {
                this.Response.StatusCode = 401;
                return Redirect("/Index");
            }
        }


        private async Task<List<OrderDBModel>> GetOrders()
        {
            return await _context.PizzaOrders.AsNoTracking().ToListAsync();
        }
        private async Task<List<PizzaDBModel>> GetPizzas()
        {
            return await _context.Pizzas.AsNoTracking().ToListAsync();
        }
    }
}
