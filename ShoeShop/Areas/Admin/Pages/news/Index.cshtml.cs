using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.news;
using DataAccess.Models;
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

        public async Task OnGetAsync()
        {
            News = (await _newsService.GetAllAsync())?.ToList() ?? new List<News>();

        }
    }
}
