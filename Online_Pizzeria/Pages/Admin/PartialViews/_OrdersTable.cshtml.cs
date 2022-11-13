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

        public string[] Statuses => Helper.GetPossibleStatuses();
        public List<OrderAdminModel> Orders { get; set; } = new List<OrderAdminModel>();

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
        private async Task<List<OrderAdminModel>> GetOrders()
        {
            var OrdersDB = await _context.PizzaOrders.AsNoTracking().ToListAsync();
            var OrdersAdmin = new List<OrderAdminModel>();

            foreach (var orderDB in OrdersDB)
            {
                var orderAdmin = new Mapper<OrderDBModel, OrderAdminModel>().Map(orderDB);
                if (orderDB.PizzaId == null)
                {
                    orderAdmin.PizzaName = "Custom Pizza";
                }
                else
                {
                    var pizza = _context.Pizzas.FirstOrDefault(x => x.Id == orderDB.PizzaId);
                    orderAdmin.PizzaName = pizza == null ? "Deleted Pizza" : pizza.Name;
                }
     
                OrdersAdmin.Add(orderAdmin);
            }

            return OrdersAdmin;
        }
    }
}
