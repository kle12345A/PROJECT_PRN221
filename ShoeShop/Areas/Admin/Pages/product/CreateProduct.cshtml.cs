using BusinessObject.category;
using BusinessObject.product;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO; 
using Microsoft.AspNetCore.Hosting;

namespace ShoeShop.Areas.Admin.Pages.product
{
    public class CreateProductModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment; 

        public CreateProductModel(
            IProductService productService,
            ICategoryService categoryService,
            IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        [BindProperty]
        public IFormFile ImageFile { get; set; } 

        public async Task OnGetAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewData["Cid"] = new SelectList(categories, "Id", "Title");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (userJson != null)
            {
                var user = System.Text.Json.JsonSerializer.Deserialize<dynamic>(userJson);
                Product.AdminCreate = user?.Name;
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

                 Product.CreateDate = DateTime.Now;
            await _productService.AddAsync(Product);

            return RedirectToPage("ProductManager");
        }
    }
}
