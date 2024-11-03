using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Models;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.Models.PROJECT_PRN212Context _context;

        public CreateModel(DataAccess.Models.PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToPage("/authentication/Login");
            }

            var authenticatedUser = JsonSerializer.Deserialize<User>(userSession);
            if (authenticatedUser.RoleId != 1)
            {
                return RedirectToPage("/AccessDenied");
            }

            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; } = default!;

       
        public async Task<IActionResult> OnPostAsync()
        {
     
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToPage("/authentication/Login");
            }

            var authenticatedUser = JsonSerializer.Deserialize<User>(userSession);
            if (authenticatedUser.RoleId != 1)
            {
                return RedirectToPage("/AccessDenied");
            }

            if (!ModelState.IsValid || _context.Contacts == null || Contact == null)
            {
                return Page();
            }

            _context.Contacts.Add(Contact);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
