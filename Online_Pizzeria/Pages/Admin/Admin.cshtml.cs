using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Online_Pizzeria.Logic;

namespace Online_Pizzeria.Pages.Admin
{
    public class AdminModel : PageModel
    {
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
        }
    }
}
