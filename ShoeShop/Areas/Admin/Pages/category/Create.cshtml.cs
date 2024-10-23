using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Models;
using BusinessObject.category;

namespace ShoeShop.Areas.Admin.Pages.category
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Category Category { get; set; } = new Category();

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public IActionResult OnGet()
        {
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

            await _categoryService.AddAsync(Category);

            return RedirectToPage("./Index");
        }
    }
}
