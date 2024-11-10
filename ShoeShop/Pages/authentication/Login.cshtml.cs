using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Security.Claims;

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

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var authenticatedUser = _userService.Authenticate(Users.Email, Users.Password);
            if (authenticatedUser != null)
            {
                if (!authenticatedUser.IsActive)
                {
                    ErrorMessage = "Your account is not active.";
                    return Page(); 
                }
                authenticatedUser.LastLogin = DateTime.Now;
                _userService.UpdateLastLoginAsync(authenticatedUser);
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

        public async Task<IActionResult> OnGetLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Page("/authentication/Login", "GoogleResponse")
            });

            return new EmptyResult();
        }

        public async Task<IActionResult> OnGetGoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal != null)
            {
               
                var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                var name = result.Principal.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;


                var existingUser = await _userService.GetByEmailAsync(email);


                if (existingUser != null)
                {
                    if (!existingUser.IsActive)
                    {
                        TempData["ErrorMessage"] = "Your account is not active.";
                        return RedirectToPage("/authentication/Login"); 
                    }

                    existingUser.LastLogin = DateTime.Now;
                    await _userService.UpdateLastLoginAsync(existingUser);
                    var userJson = JsonSerializer.Serialize(existingUser);
                    HttpContext.Session.SetString("UserSession", userJson);

                    return RedirectToPage("/Index");
                }
                else
                {

                    TempData["ErrorMessage"] = "This Google account is not registered in the system.";
                    return RedirectToPage("/authentication/Login");

                }
            }

            return RedirectToPage("/authentication/Login");
        }
    }
}
