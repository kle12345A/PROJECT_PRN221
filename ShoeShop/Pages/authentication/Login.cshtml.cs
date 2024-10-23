using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ShoeShop.Pages.authentication
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public User Users { get; set; } = new User();
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            var authenticatedUser = _userService.Authenticate(Users.Email, Users.Password);
            if (authenticatedUser != null)
            {
                var userJson = JsonSerializer.Serialize(authenticatedUser);
                HttpContext.Session.SetString("UserSession", userJson);
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
