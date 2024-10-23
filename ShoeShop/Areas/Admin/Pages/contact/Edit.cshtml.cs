using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.contact;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class EditModel : PageModel
    {
        private readonly IContactService _contactService;

        public EditModel(IContactService contactService)
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

            Contact = await _contactService.GetByIdAsync(id.Value);
            if (Contact == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _contactService.UpdateAsync(Contact);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ContactExists(Contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ContactExists(int id)
        {
            return (await _contactService.FindAsync(c => c.Id == id)).Any();
        }
    }
}
