using BusinessObject.user;
using DataAccess.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ShoeShop.Pages.authentication
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }
      
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingUser = await _userService.GetByEmailAsync(Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists. Please use a different email.");
                return Page(); 
            }

            var emailParts = Email.Split('@');
            var name = emailParts.Length > 0 ? emailParts[0] : string.Empty;

            var newUser = new User
            {
                Email = Email,
                Password = Password, 
                CreateDate = DateTime.Now,
                Name = name,
                IsActive = false,
                RoleId = 2


            };
            var activationLink = Url.Page(
                "/authentication/ActivateAccount",
                 pageHandler: null,
                  values: new { email = Email },
                  protocol: Request.Scheme
   );

           
            await SendActivationEmailAsync(Email, activationLink);

            TempData["Message"] = "A confirmation email has been sent to your email address.";

            await _userService.AddAsync(newUser);

           
            return RedirectToPage("/authentication/Login");
        }

        private async Task SendActivationEmailAsync(string email, string activationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Shoe Shop", "kienlvhe173114@fpt.edu.vn"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Activate your account";

            message.Body = new TextPart("html")
            {
                Text = $"Click the link below to activate your account: <a href='{activationLink}'>Activate Account</a>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("kienlvhe173114@fpt.edu.vn", "wcpo sbjd lzih qnrq"); 
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
