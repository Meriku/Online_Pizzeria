using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.DataBase
{
    public class ApplicationDB : DbContext
    {
        public DbSet<PizzaDBModel> PizzaOrders { get; set; }

        public ApplicationDB(DbContextOptions<ApplicationDB> options) : base(options)
        {

        }

    }
}
