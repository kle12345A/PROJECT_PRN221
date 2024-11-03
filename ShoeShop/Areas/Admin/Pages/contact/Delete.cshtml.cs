using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.contact;
using DataAccess.Models;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class DeleteModel : PageModel
    {
        private readonly IContactService _contactService;

        public DeleteModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        [BindProperty]
        public Contact Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Kiểm tra quyền truy cập
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

            // Kiểm tra ID và lấy thông tin liên hệ
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

        public async Task<IActionResult> OnPostAsync(int? id)
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

            await _contactService.DeleteAsync(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
