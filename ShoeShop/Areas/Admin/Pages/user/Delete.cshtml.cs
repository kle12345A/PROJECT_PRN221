using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.user;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }

            if (!await CheckAccessAsync())
            {
                Redirect("AccesDenied");

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(id.Value);

            if (user != null)
            {
                User = user;

                if (!await CheckAccessAsync())
                {
                    return Forbid(); // Hoặc Redirect đến trang khác nếu không có quyền
                }

                await _userService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> CheckAccessAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return false;
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<User>(userSession);
            return user?.RoleId == 1; // Kiểm tra quyền truy cập
        }
    }
}
