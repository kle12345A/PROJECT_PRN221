using X.PagedList;
using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList.Extensions;

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
        public IPagedList<News> News { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1; 

        public async Task OnGetAsync()
        {
            int pageSize = 6;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
               
                var searchResults = await _newsService.SearchAsync(SearchTerm);
                var activeSearchResults = searchResults.Where(news => news.Status == true).OrderByDescending(x => x.CreateDate);
                News = activeSearchResults.ToPagedList(PageNumber, pageSize);
            }
            else
            {
              
                var allNews = await _newsService.GetAllAsync();
                var activeNews = allNews.Where(news => news.Status == true).OrderByDescending(x => x.CreateDate); ;
                News = activeNews.ToPagedList(PageNumber, pageSize);
            }
        }
    }
}
