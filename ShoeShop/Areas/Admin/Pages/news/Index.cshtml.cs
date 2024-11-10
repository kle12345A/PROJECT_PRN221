using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Areas.Admin.Pages.news
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        public List<News> News { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession))
            {
                return RedirectToPage("/authentication/Login");
            }

            var authenticatedUser = JsonSerializer.Deserialize<User>(userSession);
            if (authenticatedUser.RoleId != 1)
            {
                return RedirectToPage("/AccessDenied");
            }

            News = (await _newsService.GetAllAsync())?.ToList() ?? new List<News>();
            return Page();
        }
    }
}
