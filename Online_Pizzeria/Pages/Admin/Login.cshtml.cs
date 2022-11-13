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
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var sessionIdExisting = this.Request.Cookies["sessionId"];
            if (Sessions.CheckSessionId(sessionIdExisting))
            {
                return Redirect($"/Admin/Admin");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Sessions.Login(_configuration, Login, Password, out var sessionId))
            {
                _logger.LogTrace($"Logged in successfully::Log {DateTime.Now:G}");
                Response.Cookies.Append("sessionId", $"{sessionId}");
                return Redirect($"/Admin/Admin");
            }
            else
            {
                _logger.LogError($"Login attempt failed::Log {DateTime.Now:G}");
                Response.StatusCode = 401;
                return RedirectToPage("/Index");
            }
        }
    }
}
