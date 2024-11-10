using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace ShoeShop.Areas.Admin.Pages.order
{
    public class DetailsModel : PageModel
    {
        private readonly PROJECT_PRN212Context _context;

        public DetailsModel(PROJECT_PRN212Context context)
        {
            _context = context;
        }

        public List<dynamic> OrderDetails { get; set; } = new List<dynamic>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
       
            if (!await CheckAccessAsync())
            {
                return RedirectToPage("/AccessDenied"); 
            }

            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await (from order in _context.Orders
                                      join detail in _context.OrderDetails on order.Id equals detail.OrderId
                                      join product in _context.Products on detail.ProductId equals product.Id
                                      where order.Id == id
                                      select new
                                      {
                                          OrderId = order.Id,
                                          OrderDate = order.UpdateDate,
                                          CustomerName = order.Name,
                                          Email = order.Email,
                                          Address = order.Address,
                                          ProductId = product.Id,
                                          ProductName = product.Title,
                                          Quantity = detail.Quantity,
                                          UnitPrice = detail.Price,
                                          ProductImage = product.Image,
                                          Status = order.Status
                                      }).ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            OrderDetails = orderDetails.Select(o => (dynamic)o).ToList();
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
