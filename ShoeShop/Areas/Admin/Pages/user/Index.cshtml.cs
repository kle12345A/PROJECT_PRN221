using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<User> Users { get; private set; } = new List<User>();

        public async Task OnGetAsync()
        {
            if (!await CheckAccessAsync())
            {
                RedirectToPage("/AccessDenied");
                return;
            }

            Users = await _userService.GetAllWithRolesAsync();
        }

        private async Task<bool> CheckAccessAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return false;
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<User>(userSession);
            return user?.RoleId == 1;
        }
    }
}
