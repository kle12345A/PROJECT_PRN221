using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Pages.news
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        [BindProperty]
        public List<News> News { get; set; } = new List<News>();

        public async Task OnGetAsync()
        {
            News = (await _newsService.GetAllAsync()).ToList();
        }
    }
}
