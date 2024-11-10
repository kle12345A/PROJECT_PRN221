using BusinessObject.user;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Pages.authentication
{
        public class ResetPasswordModel : PageModel
        {
            private readonly IUserService _userService;

            public ResetPasswordModel(IUserService userService)
            {
                _userService = userService;
            }

            [BindProperty]
            public ResetPasswordInputModel Input { get; set; }

            [TempData]
            public string StatusMessage { get; set; }

            public class ResetPasswordInputModel
            {
                [Required]
                [EmailAddress]
                public string Email { get; set; }

                [Required]
                [DataType(DataType.Password)]
                public string NewPassword { get; set; }

                [Required]
                [DataType(DataType.Password)]
                [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
                public string ConfirmPassword { get; set; }
            }

            public IActionResult OnGet(string email = null)
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email must be supplied for password reset.");
                }

                Input = new ResetPasswordInputModel
                {
                    Email = email
                };
                return Page();
            }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetByEmailAsync(Input.Email);
            if (user == null)
            {
                StatusMessage = "Email address not found.";
                return RedirectToPage("/authentication/Login");
            }
            user.UpdateDate = DateTime.Now;
            var updateResult = await _userService.UpdatePasswordAsync(user, Input.NewPassword);
            if (updateResult)
            {
                HttpContext.Session.Remove("UserSession");
                TempData["Message"] = "Password reset successfully.";
                return RedirectToPage("/authentication/Login");
            }

            StatusMessage = "Failed to reset the password.";
            return Page();
        }

    }
}



