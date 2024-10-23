using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.role
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Models.PROJECT_PRN212Context _context;

        public IndexModel(DataAccess.Models.PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public IList<Role> Role { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Roles != null)
            {
                Role = await _context.Roles.ToListAsync();
            }
        }
    }
}
