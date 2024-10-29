using BusinessObject.category;
using BusinessObject.product;
using DataAccess.Models;
using DataAccess.Repository.category;
using DataAccess.Repository.product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeShop.Pages.product
{
    public class ListModel : PageModel
    {
        private IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ListModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int? PriceFilter { get; set; }

        public async Task OnGetAsync(int id, int? price, string searchTerm)
        {
            CategoryId = id;

            var category = await _categoryService.GetByIdAsync(id);
            CategoryName = category?.Title;

            // L?y danh sách s?n ph?m theo ID danh m?c
            Products = (List<Product>)_productService.GetProductsByCategoryId(id);

            // Áp d?ng b? l?c giá
            if (price.HasValue)
            {
                if (price.Value == 0)
                {
                    // Không l?c, l?y t?t c? s?n ph?m
                }
                else if (price.Value <= 5000000)
                {
                    Products = Products.Where(p => p.PriceNew < 5000000).ToList();
                }
                else if (price.Value <= 10000000)
                {
                    Products = Products.Where(p => p.PriceNew >= 5000000 && p.PriceNew < 10000000).ToList();
                }
                else if (price.Value <= 15000000)
                {
                    Products = Products.Where(p => p.PriceNew >= 10000000 && p.PriceNew < 15000000).ToList();
                }
                else
                {
                    Products = Products.Where(p => p.PriceNew > 15000000).ToList();
                }
            }

            // Áp d?ng tìm ki?m theo tên
            if (!string.IsNullOrEmpty(searchTerm))
            {
                Products = Products.Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

    }
}
