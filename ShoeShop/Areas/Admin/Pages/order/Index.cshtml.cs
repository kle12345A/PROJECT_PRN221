using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.order
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Models.PROJECT_PRN212Context _context;

        public IndexModel(DataAccess.Models.PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra quyền truy cập
            var accessResult = await CheckAccessAsync();
            if (accessResult != null) // Nếu có trang để chuyển hướng
            {
                return accessResult;
            }

            if (_context.Orders != null)
            {
                Order = await _context.Orders
                    .Include(o => o.User)
                    .ToListAsync();
            }
            return Page();
        }

        private async Task<IActionResult> CheckAccessAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                
                return RedirectToPage("/Authentication/Login");
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<User>(userSession);
            if (user?.RoleId != 1) 
            {
                return RedirectToPage("/AccessDenied");
            }
            return null;
        }
    }
}
