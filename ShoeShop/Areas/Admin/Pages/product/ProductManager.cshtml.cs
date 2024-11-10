using BusinessObject.product;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.category;

namespace ShoeShop.Areas.Admin.Pages.product
{
    public class ProductManagerModel : PageModel
    {
        private  IProductService _productService;
        private ICategoryService _categoryService;

        public ProductManagerModel(IProductService productService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CategoryFilter { get; set; }

        [BindProperty]
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; }
        public bool HasAccess { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            HasAccess = await CheckAccessAsync();

            if (!HasAccess)
            {
                return Redirect("Authentication/Login");
            }

            var allProducts = await _productService.GetAllAsync();
            Categories = (await _categoryService.GetAllAsync()).ToList();

            Products = allProducts
       .Where(p =>
           (!CategoryFilter.HasValue || p.Cid == CategoryFilter.Value) && 
           (string.IsNullOrEmpty(SearchString) || p.Title.Contains(SearchString, StringComparison.OrdinalIgnoreCase))) 
       .ToList();

            return Page();
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
    }
}
