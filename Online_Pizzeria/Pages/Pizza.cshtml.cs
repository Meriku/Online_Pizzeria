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
        private readonly Mapper<PizzaDBModel, PizzaUserModel> _mapper = new();

        public PizzaModel(ApplicationDB context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var pizzasDB = _context.Pizzas.ToArray();
            pizzasUser = new List<PizzaUserModel>();

            if (pizzasDB == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var pizza in pizzasDB)
            {
                pizzasUser.Add(_mapper.Map(pizza));
            }
        }





    }
}
