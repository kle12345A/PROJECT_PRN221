using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.news;
using DataAccess.Models;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.news
{
    public class DetailsModel : PageModel
    {
        private readonly INewsService _newsService;

        public DetailsModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        public News News { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession) || !JsonSerializer.Deserialize<User>(userSession).RoleId.Equals(1))
            {
                return RedirectToPage("/AccessDenied");
            }

            if (id == null || _newsService == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetByIdAsync(id.Value);
            if (news == null)
            {
                return NotFound();
            }
            else
            {
                News = news;
            }
            return Page();
        }
    }
}
