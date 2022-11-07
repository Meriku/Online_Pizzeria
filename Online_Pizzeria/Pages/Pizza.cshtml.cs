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
            PizzasNames = _context.Pizzas.Select(x => x.Name).ToArray();
            foreach (var pizza in PizzasNames)
            {
                var imgsrc = @$"wwwroot/images/AllPizzas/{pizza}.png";
                if (!System.IO.File.Exists(imgsrc))
                {
                    System.IO.File.Copy(@"wwwroot/images/AllPizzas/Create.png", imgsrc);
                    Console.WriteLine($"Default image was created for {pizza}::LOG {DateTime.Now:G}");
                }
            }
        }
    }
}
