using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.contact;
using DataAccess.Models;

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
