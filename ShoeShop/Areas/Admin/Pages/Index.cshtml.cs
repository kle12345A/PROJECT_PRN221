using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (userSession == null)
            {
                Response.Redirect("/authentication/Login");
                return;
            }

            var authenticatedUser = JsonSerializer.Deserialize<User>(userSession);
            if (authenticatedUser.RoleId != 1)
            {
                Response.Redirect("/AccessDenied");
            }
        }
    }
}
