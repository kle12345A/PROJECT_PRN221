using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment; 
        private readonly PROJECT_PRN212Context _context;
        public CreateModel(IUserService userService, IWebHostEnvironment environment, PROJECT_PRN212Context context)
        {
            _userService = userService;
            _environment = environment;
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        
       

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || User == null)
            {
                return Page();
            }


            User.CreateDate = DateTime.Now;

            _context.Users.Add(User);
            await _context.SaveChangesAsync(); 

            return RedirectToPage("./Index"); 
        }
    }
}
