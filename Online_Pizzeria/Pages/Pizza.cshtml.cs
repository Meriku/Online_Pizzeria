using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;
using Online_Pizzeria.Models;

namespace Online_Pizzeria.Pages.Forms
{
    public class PizzaModel : PageModel
    {
        public string[] PizzasNames;

        private readonly ApplicationDB _context;
        private readonly Mapper<OrderUserModel, OrderDBModel> _mapper;

        public PizzaModel(ApplicationDB context)
        {
            _context = context;
            _mapper = new Mapper<OrderUserModel, OrderDBModel>();
        }

        public void OnGet()
        {
            PizzasNames ??= new string[] { "Bolognese", "Carbonara", "Hawaiian", "Margerita", "MeatFeast", "Mushroom", "Pepperoni", "Seafood", "Vegetarian" };
            


        }
    }
}
