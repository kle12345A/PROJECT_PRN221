using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.product;
using BusinessObject.category;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ShoeShop.Areas.Admin.Pages.product
{
    public class UpdateProductModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public UpdateProductModel(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!await CheckAccessAsync())
            {
                return RedirectToPage("/AccessDenied");
            }

            if (id == null || _productService == null)
            {
                return NotFound();
            }

            Product = await _productService.GetByIdAsync(id.Value);
            if (Product == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllAsync();
            ViewData["Cid"] = new SelectList(categories, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
       
            if (!await CheckAccessAsync())
            {
                return RedirectToPage("/AccessDenied");
            }

            var userJson = HttpContext.Session.GetString("UserSession");
            if (userJson != null)
            {
                var user = System.Text.Json.JsonSerializer.Deserialize<dynamic>(userJson);
                Product.AdminUpdate = user?.Name;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageFile != null)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var uploadPath = Path.Combine(_environment.WebRootPath, "Image", "product");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Product.Image = $"/Image/product/{fileName}";
            }
            else
            {
                Product.Image = Request.Form["CurrentImage"];
            }

            Product.UpdateDate = DateTime.Now;

            await _productService.UpdateAsync(Product);

            return RedirectToPage("ProductManager");
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

        private async Task<bool> ProductExists(int id)
        {
            return (await _productService.FindAsync(m => m.Id == id)).Any();
        }
    }
}
