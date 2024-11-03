using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Models;
using BusinessObject.contact;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class IndexModel : PageModel
    {
        private readonly IContactService _contactService;

        public IndexModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public List<Contact> Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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

            Contact = (await _contactService.GetAllAsync())?.ToList() ?? new List<Contact>();
            return Page();
        }
    }
}
