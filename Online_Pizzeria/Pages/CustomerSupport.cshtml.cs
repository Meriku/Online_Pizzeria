using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Online_Pizzeria.Pages
{
    public class CustomerSupport : PageModel
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        private readonly IConfiguration _configuration;
        public CustomerSupport(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            PhoneNumber = _configuration.GetValue<string>("Company:PhoneNumber") ?? "Change in AppSettings";
            Address = _configuration.GetValue<string>("Company:Address") ?? "Change in AppSettings";
            Email = _configuration.GetValue<string>("Company:Email") ?? "default@company.com";
        }
    }
}