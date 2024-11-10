using BusinessObject.order;
using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IOrdersService _ordersService;
        public IUserService _userService;
        public IndexModel(IOrdersService ordersService, IUserService userService)
        {
            _userService = userService;
        _ordersService = ordersService; 
        }
        public int TotalOrdersCount { get; private set; }
        public decimal TotalPriceByStatus { get; private set; }
        public int PendingOrdersCount { get; private set; }
        public int TotalUsersCount { get; private set; }
        public List<Dictionary<string, object>> TotalQuantityPerProduct { get; set; }
        public async Task OnGetAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (userSession == null)
            {
                Response.Redirect("/authentication/Login");
                return;
            }

            var authenticatedUser = JsonSerializer.Deserialize<User>(userSession);
            if (authenticatedUser.RoleId != 1)
            {
                Response.Redirect("/AccessDenied");
            }

            TotalOrdersCount = await _ordersService.GetTotalOrdersCountAsync();
            TotalPriceByStatus = await _ordersService.GetTotalPriceByStatusAsync("1");
            PendingOrdersCount = await _ordersService.GetPendingOrdersCountAsync("0");
            TotalUsersCount = await _userService.GetTotalUsersCountAsync();
            TotalQuantityPerProduct = await _ordersService.GetTotalQuantityPerProductAsync();
        }
    }
}
