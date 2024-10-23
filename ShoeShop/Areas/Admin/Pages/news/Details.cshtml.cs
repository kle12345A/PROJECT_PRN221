using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.contact;
using BusinessObject.news;

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
