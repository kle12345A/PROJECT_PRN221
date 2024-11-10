using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BusinessObject.user;

namespace ShoeShop.Pages.authentication
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserService _userService;
        public ForgotPasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Message {  get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetByEmailAsync(Email);
            if (user == null)
            {
                Message = "Email does not exist in the system";
               
                return Page();
            }

            var resetLink = Url.Page( "/authentication/ResetPassword", pageHandler: null, values: new { email = Email }, protocol: Request.Scheme );

            Console.WriteLine(resetLink);

            await SendResetEmailAsync(Email, resetLink);

            TempData["Message"] = "A password reset link has been sent to your email.";
            return RedirectToPage("/authentication/Login");
        }

        private async Task SendResetEmailAsync(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Shoe Shop", "kienlvhe173114@fpt.edu.vn"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Reset your password";

            message.Body = new TextPart("html")
            {
                Text = $"Click on the following link to reset your password: <a href='{resetLink}'>Reset Password</a>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("kienlvhe173114@fpt.edu.vn", "wcpo sbjd lzih qnrq");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
