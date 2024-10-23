using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.contact;

namespace ShoeShop.Areas.Admin.Pages.contact
{
    public class IndexModel : PageModel
    {
        private readonly IContactService _contactService;
        public IndexModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public List<Contact> Contact { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Contact = (await _contactService.GetAllAsync())?.ToList() ?? new List<Contact>();

        }
    }
}
