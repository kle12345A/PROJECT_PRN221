using BusinessObject.order;
using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ShoeShop.Pages.profile
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IOrdersService _ordersService;

        public IndexModel(IUserService userService, IOrdersService ordersService)
        {
            _ordersService = ordersService;
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<List<OrderDetail>> OrderDetails { get; set; } = new List<List<OrderDetail>>();

     
        public int? SelectedOrderId { get; set; }

        public async Task OnGetAsync(int id, int? orderId = null)
        {
            User = await _userService.GetByIdAsync(id);
            Orders = await _ordersService.GetQuery().Include(x => x.OrderDetails).Where(x => x.UserId == id).ToListAsync();
            await LoadOrderDetailsAsync();
            SelectedOrderId = orderId;
        }

        public async Task LoadOrderDetailsAsync()
        {
            foreach (var order in Orders)
            {
                var details = await _ordersService.GetOrderDetailsAsync(order.Id);
                OrderDetails.Add(details);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            try
            {
                await _userService.UpdateAsync(User);
                return RedirectToPage("./Index", new { id = User.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có l?i x?y ra khi c?p nh?t thông tin ng??i dùng. Vui lòng th? l?i.");
                return Page(); 
            }
        }
    }


}
