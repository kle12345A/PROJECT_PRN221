using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ShoeShop.Pages.cart
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN212Context _context;
        private const string CartSessionKey = "My-Cart";

        public List<Cart> Carts { get; set; } = new();
        public decimal Total => Carts.Sum(item => item.Total); // T�nh t?ng tr?c ti?p t? danh s�ch

        public IndexModel(PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString(CartSessionKey);
            if (cartInSession != null)
            {
                Carts = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            base.OnPageHandlerExecuting(context);
        }

        public void OnGet()
        {
            // T?ng gi� tr? gi? h�ng ?� ???c t�nh to�n trong thu?c t�nh Total
        }

        public IActionResult OnGetAdd(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cartItem = Carts.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                cartItem.Quantity++; // T?ng s? l??ng
            }
            else
            {
                // Th�m s?n ph?m m?i v�o gi? h�ng
                Carts.Add(new Cart
                {
                    Id = product.Id,
                    Name = product.Title,
                    Price = product.PriceNew.Value,
                    Quantity = 1,
                    Image = product.Image
                });
            }

            SaveCart();
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int id, int quantity)
        {
            var cartItem = Carts.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                    cartItem.Total = cartItem.Price * quantity; // C?p nh?t t?ng
                }
                else
                {
                    Carts.Remove(cartItem); // X�a s?n ph?m n?u s? l??ng kh�ng h?p l?
                }
            }

            SaveCart();
            return RedirectToPage();
        }


        public IActionResult OnGetRemove(int id)
        {
            var cartItem = Carts.FirstOrDefault(c => c.Id == id);
            if (cartItem != null)
            {
                Carts.Remove(cartItem); // X�a s?n ph?m kh?i gi? h�ng
                SaveCart();
            }
            return RedirectToPage();
        }

        private void SaveCart()
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(Carts)); // L?u gi? h�ng v�o session
        }
    }
}
