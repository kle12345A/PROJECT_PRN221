﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.category;

namespace ShoeShop.Areas.Admin.Pages.category
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _categoryService == null)
            {
                return NotFound();
            }

            Category = await _categoryService.GetByIdAsync(id.Value);
            if (Category == null)
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

         
            if (ImageFile != null)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var uploadPath = Path.Combine(_environment.WebRootPath, "Image", "category");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Category.Icon = $"/Image/category/{fileName}";
            }

            await _categoryService.UpdateAsync(Category); 

            return RedirectToPage("./Index"); 
        }
    }
}
