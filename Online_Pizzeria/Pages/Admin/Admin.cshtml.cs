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
        public DbSet<OrderDBModel> PizzaOrders;
        public DbSet<PizzaDBModel> Pizzas;


        public AdminModel(ApplicationDB context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var sessionId = this.Request.Query["sessionId"];

            if (Sessions.CheckSessionId(sessionId))
            {
                this.Response.StatusCode = 200;
            }
            else
            {
                this.Response.StatusCode = 401;
                Response.Redirect("/Error");
            }


            PizzaOrders = _context.PizzaOrders;
            Pizzas = _context.Pizzas;
        }
    }
}
