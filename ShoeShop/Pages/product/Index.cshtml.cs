using BusinessObject.category;
using BusinessObject.product;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Pages.product
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public List<Product> Products { get; set; }
        public Dictionary<int, List<Product>> ProductsByCategory { get; set; }
        public Dictionary<int, string> CategoryNames { get; set; }

        public async Task OnGet()
        {
            var allProducts = await _productService.GetAllAsync();
            var allCategories = await _categoryService.GetAllAsync();

            

            ProductsByCategory = allProducts
                .Where(p => p.Cid.HasValue)
                .GroupBy(p => p.Cid.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.Take(4).ToList() 
                );

            CategoryNames = allCategories.ToDictionary(c => c.Id, c => c.Title);
        }
    }
}
