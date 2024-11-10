using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.user;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;

        public DetailsModel(IUserService userService)
        {
            _userService = userService;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userService.GetByIdAsync(id.Value);
            if (User == null)
            {
                return NotFound();
            }

            if (!await CheckAccessAsync())
            {
                Redirect("AccesDenied");

            }

            return Page();
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
