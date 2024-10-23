using BusinessObject.user;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeShop.Areas.Admin.Pages.user
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        // Sử dụng Property thay vì BindProperty vì không cần thiết
        public IList<User> Users { get; private set; } = new List<User>();

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllWithRolesAsync();
        }
    }
}
