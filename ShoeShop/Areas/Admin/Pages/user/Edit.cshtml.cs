using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;
        private readonly PROJECT_PRN212Context _context;

        public EditModel(IUserService userService, IWebHostEnvironment environment, PROJECT_PRN212Context context)
        {
            _userService = userService;
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            User = user;
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(User).State = EntityState.Modified;
            User.UpdateDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<bool> CheckAccessAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return false;
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<User>(userSession);
            return user?.RoleId == 1;
        }
    }
}
