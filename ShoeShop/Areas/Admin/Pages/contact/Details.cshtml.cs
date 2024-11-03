using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.contact;
using DataAccess.Models;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class DetailsModel : PageModel
    {
        private readonly IContactService _contactService;

        public DetailsModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public Contact Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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

          
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _contactService.GetByIdAsync(id.Value);
            if (contact == null)
            {
                return NotFound();
            }

            Contact = contact;
            return Page();
        }
    }
}
