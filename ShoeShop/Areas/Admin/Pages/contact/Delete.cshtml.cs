using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.contact;
using DataAccess.Models;

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
