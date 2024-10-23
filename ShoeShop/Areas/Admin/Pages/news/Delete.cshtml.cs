using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.news;

namespace ShoeShop.Areas.Admin.Pages.news
{
    public class DeleteModel : PageModel
    {
        private readonly INewsService _newService;

        public DeleteModel(INewsService newsService)
        {
             _newService = newsService;
        }

        [BindProperty]
      public News News { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _newService == null)
            {
                return NotFound();
            }

            var news = await _newService.GetByIdAsync(id.Value);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _newService == null)
            {
                return NotFound();
            }
            var news = await _newService.GetByIdAsync(id.Value);

            if (news != null)
            {
                
              
                await _newService.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
