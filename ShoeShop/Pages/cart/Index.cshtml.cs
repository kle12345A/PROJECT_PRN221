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
        private const string CustomerSessionKey = "UserSession";
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
                cartItem.Quantity++; 
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

        public IActionResult OnPostCheckout()
        {
            // Ki?m tra n?u gi? h�ng r?ng
            if (Carts == null || !Carts.Any())
            {
                TempData["ErrorMessage"] = "Gi? h�ng tr?ng!";
                return RedirectToPage(); // Quay l?i trang gi? h�ng
            }

            // Ki?m tra th�ng tin kh�ch h�ng t? session
            var customerJson = HttpContext.Session.GetString(CustomerSessionKey);
            if (string.IsNullOrEmpty(customerJson))
            {
                return RedirectToPage("/Login/Index"); // Chuy?n h??ng ??n trang ??ng nh?p
            }

            var customer = JsonConvert.DeserializeObject<User>(customerJson);

            // T?o ??n h�ng
            var order = new Order
            {
                UserId = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                UpdateDate = DateTime.Now,
                Status = "0" // ??n h�ng m?i
            };
            _context.Orders.Add(order);
            _context.SaveChanges(); // L?u ?? l?y ID ??n h�ng

            // T?o chi ti?t ??n h�ng
            foreach (var item in Carts)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges(); // L?u t?t c? thay ??i

            // X�a gi? h�ng kh?i session
            HttpContext.Session.Remove(CartSessionKey);

            TempData["SuccessMessage"] = "??t h�ng th�nh c�ng!";
            return RedirectToPage("/Cart/Thanks"); // ?i?u h??ng ??n trang c?m ?n
        }


        private void SaveCart()
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(Carts)); // L?u gi? h�ng v�o session
        }
    }
}
