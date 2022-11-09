using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Admin.PartialViews
{
    public class _OrdersTableModel : PageModel
    {
        private readonly ApplicationDB _context;

        public List<OrderDBModel> Orders { get; set; } = new List<OrderDBModel>();

        public _OrdersTableModel(ApplicationDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionId = this.Request.Cookies["sessionId"];

            if (Sessions.CheckSessionId(sessionId))
            {
                Orders = await GetOrders();
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
    }
}
