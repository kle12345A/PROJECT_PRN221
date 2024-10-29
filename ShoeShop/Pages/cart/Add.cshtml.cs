using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ShoeShop.Pages.cart
{
    public class AddModel : PageModel
    {
        private readonly PROJECT_PRN212Context _context;
        private const string CartSessionKey = "My-Cart";

        public AddModel(PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            var carts = cartJson != null
                ? JsonConvert.DeserializeObject<List<Cart>>(cartJson)
                : new List<Cart>();

            var existingItem = carts.FirstOrDefault(c => c.Id == id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.Total = existingItem.Quantity * existingItem.Price;
            }
            else
            {
                carts.Add(new Cart
                {
                    Id = product.Id,
                    Name = product.Title,
                    Price = product.PriceNew.Value,
                    Quantity = 1,
                    Image = product.Image,
                    Total = product.PriceNew.Value
                });
            }

            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(carts));
            return RedirectToPage("/Cart/Index");
        }
    }
}
