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
        public DbSet<OrderDBModel> PizzaOrders { get; set; }
        public DbSet<PizzaDBModel> Pizzas { get; set; }
        

        public AdminModel(ApplicationDB context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var sessionId = this.Request.Cookies["sessionId"];
            IsOrders = this.Request.Query["view"].Equals("Orders");

            if (Sessions.CheckSessionId(sessionId))
            {
                this.Response.StatusCode = 200;
                if (IsOrders)
                {
                    PizzaOrders = _context.PizzaOrders;
                }
                else
                {
                    Pizzas = _context.Pizzas;
                }
            }
            else
            {
                this.Response.StatusCode = 401;
                Response.Redirect("/Index");
            }

      
        }
    }
}
