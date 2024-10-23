﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.product;
using BusinessObject.category;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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




        private async Task<bool> ProductExists(int id)
        {
           
            return (await _productService.FindAsync(m => m.Id == id)).Any();
        }
    }
}
