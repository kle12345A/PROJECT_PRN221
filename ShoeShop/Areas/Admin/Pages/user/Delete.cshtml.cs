﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.user;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
      public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }
            else 
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }
            var user = await _userService.GetByIdAsync(id.Value);

            if (user != null)
            {
                User = user;
                await _userService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
