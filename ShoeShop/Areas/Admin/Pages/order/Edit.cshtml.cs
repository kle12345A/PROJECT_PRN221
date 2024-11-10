﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.order
{
    public class EditModel : PageModel
    {
        private readonly DataAccess.Models.PROJECT_PRN212Context _context;

        public EditModel(DataAccess.Models.PROJECT_PRN212Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Kiểm tra quyền truy cập
            var accessResult = await CheckAccessAsync();
            if (accessResult != null) 
            {
                return accessResult;
            }

            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
     
            var accessResult = await CheckAccessAsync();
            if (accessResult != null) 
            {
                return accessResult;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
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

        private async Task<IActionResult> CheckAccessAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToPage("/AccessDenied");
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<User>(userSession);
            if (user?.RoleId != 1) 
            {
                return RedirectToPage("/AccessDenied");
            }
            return null;
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
