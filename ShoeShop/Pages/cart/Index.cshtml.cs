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
        public decimal Total => Carts.Sum(item => item.Total);

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
          
        }
		public IActionResult OnPostAdd(int id, int quantity)
		{
			var product = _context.Products.Find(id);
			if (product == null) return NotFound();

			if (quantity > product.Quantity)
			{
				TempData["ErrorMessage"] = "The requested quantity exceeds the available stock.";
				return RedirectToPage("/Product/Details", new { id = id });
			}

			var cartItem = Carts.FirstOrDefault(c => c.Id == id);
			if (cartItem != null)
			{
				if (cartItem.Quantity + quantity > product.Quantity)
				{
					TempData["ErrorMessage"] = "The total quantity in the cart exceeds the available stock.";
					return RedirectToPage("/Product/Details", new { id = id });
				}
				cartItem.Quantity += quantity;
			}
			else
			{
				Carts.Add(new Cart
				{
					Id = product.Id,
					Name = product.Title,
					Price = product.PriceNew.Value,
					Quantity = quantity,
					Image = product.Image
				});
			}

			SaveCart();
			return RedirectToPage();
		}

		public IActionResult OnGetAdd(int id)
		{
			var product = _context.Products.Find(id);
			if (product == null) return NotFound();

			var cartItem = Carts.FirstOrDefault(c => c.Id == id);
			if (cartItem != null)
			{
				
				if (cartItem.Quantity + 1 > product.Quantity)
				{
					TempData["ErrorMessage"] = "The total quantity in the cart exceeds the available stock.";
					return RedirectToPage();
				}
				cartItem.Quantity++;
			}
			else
			{
				if (product.Quantity < 1)
				{
					TempData["ErrorMessage"] = "The product is out of stock.";
					return RedirectToPage();
				}

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
			var product = _context.Products.Find(id);
			if (cartItem != null && product != null)
			{
				
				if (quantity > product.Quantity)
				{
					TempData["ErrorMessage"] = "The requested quantity exceeds the available stock.";
					return RedirectToPage();
				}

				if (quantity > 0)
				{
					cartItem.Quantity = quantity;
					cartItem.Total = cartItem.Price * quantity;
				}
				else
				{
					Carts.Remove(cartItem);
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
                Carts.Remove(cartItem); 
                SaveCart();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            if (Carts == null || !Carts.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống!";
                return RedirectToPage();
            }

            var customerJson = HttpContext.Session.GetString(CustomerSessionKey);
            if (string.IsNullOrEmpty(customerJson))
            {
                return RedirectToPage("/authentication/Login");
            }

            var customer = JsonConvert.DeserializeObject<User>(customerJson);

            var order = new Order
            {
                UserId = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                UpdateDate = DateTime.Now,
                Status = "0"
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in Carts)
            {
                var product = _context.Products.Find(item.Id);
                if (product != null)
                {
                    if (product.Quantity >= item.Quantity)
                    {
                        product.Quantity -= item.Quantity;

                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Sản phẩm {product.Title} không đủ số lượng!";
                        return RedirectToPage();
                    }
                }
            }

            _context.SaveChanges();

            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToPage("/Cart/Thanks");
        }



        private void SaveCart()
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(Carts));
        }
    }
}
