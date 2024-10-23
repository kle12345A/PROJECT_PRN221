using BusinessObject.product;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoeShop.Areas.Admin.Pages.product
{
    public class ProductManagerModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductManagerModel(IProductService productService) 
        {
            _productService = productService;
        }

        [BindProperty]
        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = (await _productService.GetAllAsync())?.ToList() ?? new List<Product>();
        }
    }
}
