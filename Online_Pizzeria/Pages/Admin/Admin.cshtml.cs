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
        public List<OrderDBModel> PizzaOrders { get; set; } = new List<OrderDBModel>();
        public List<PizzaDBModel> Pizzas { get; set; } = new List<PizzaDBModel>();

        public string[] Ingedients => Helper.GetPossibleIngredients();

        [BindProperty]
        public PizzaDBModel CreatePizza { get; set; }


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

        public IActionResult OnPost()
        {
            var sessionId = this.Request.Cookies["sessionId"];
            if (Sessions.CheckSessionId(sessionId))
            {
                if (CreatePizza != null)
                {
                    _context.Pizzas.Add(CreatePizza);
                    _context.SaveChanges();
                }

                return Redirect($"/Admin/Admin?view=Pizzas");
            }
            else
            {
                this.Response.StatusCode = 401;
                return RedirectToPage("/Index");
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
