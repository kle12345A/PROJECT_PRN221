using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.product;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.product
{
    public class DeleteProductModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteProductModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _productService == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id.Value);
            if (product != null)
            {
                await _productService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./ProductManager");
        }
    }
}
