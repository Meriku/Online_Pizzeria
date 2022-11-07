using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.Logic;
using System.Net;

namespace Online_Pizzeria.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Login { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (Sessions.Login(_configuration, Login, Password, out var sessionId))
            {
                Console.WriteLine($"Logged in successfully::Log {DateTime.Now:G}");

                Response.Cookies.Append("sessionId", $"{sessionId}");
                return (Redirect($"/Admin/Admin?view=Orders"));
            }
            else
            {
                Console.WriteLine($"Login attempt failed::Log {DateTime.Now:G}");
                return RedirectToPage("/Error");
            }
        }
    }
}
