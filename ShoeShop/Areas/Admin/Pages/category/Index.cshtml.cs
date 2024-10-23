using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using BusinessObject.category;

namespace ShoeShop.Areas.Admin.Pages.category
{
    public class IndexModel : PageModel
    {
        private ICategoryService _categoryService;
        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> Category { get; set; } = new List<Category>();

        public async Task OnGetAsync()
        {
            Category = (List<Category>)(await _categoryService.GetAllAsync());
        }
    }
}
