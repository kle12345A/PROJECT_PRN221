using BusinessObject.product;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        [BindProperty]
        public List<Product> Products { get; set; }
        public IndexModel( IProductService productService)
        {
            
            _productService = productService;
        }


        public async Task OnGetAsync()
        {
            var products = await _productService.GetAllAsync(); 
            ViewData["Products"] = products;
        }


    }
}
