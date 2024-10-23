using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Pages.news
{
    public class NewsDetailModel : PageModel
    {
        private readonly INewsService _newsService;
        public NewsDetailModel(INewsService newsService)
        {
            _newsService = newsService;
        }
        [BindProperty]
        public News News { get; set; } = new News();
        public List<News> NewsLeast { get; set; } = new List<News>();
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           News = await _newsService.GetByIdAsync(id.Value);
            var allNews = await _newsService.GetAllAsync();
            NewsLeast = allNews.OrderByDescending(n => n.Id).Take(3).ToList();

            if (News == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
