using BusinessObject.user;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Pages.authentication
{
    public class ActivateAccountModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty(SupportsGet =true)]
        public string Email { get; set; }

        public ActivateAccountModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

          
            var user = await _userService.GetByEmailAsync(Email);
            if (user == null || user.IsActive)
            {
                return NotFound(); 
            }

            // Kích hoạt tài khoản
            user.IsActive = true;
            await _userService.UpdateAsync(user);

            TempData["Message"] = "Your account has been successfully activated!";
            return RedirectToPage("/authentication/Login");
        }
    }

}
