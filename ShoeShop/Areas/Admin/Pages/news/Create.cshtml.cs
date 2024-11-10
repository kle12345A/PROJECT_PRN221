using System;
using System.IO;
using System.Threading.Tasks;
using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ShoeShop.Areas.Admin.Pages.news
{
    public class CreateModel : PageModel
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(INewsService newsService, IWebHostEnvironment environment)
        {
            _newsService = newsService;
            _environment = environment;
        }

        [BindProperty]
        public News News { get; set; } = new News();

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public IActionResult OnGet()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession) || JsonSerializer.Deserialize<User>(userSession)?.RoleId != 1)
            {
                return RedirectToPage("/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userSession = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userSession) || JsonSerializer.Deserialize<User>(userSession)?.RoleId != 1)
            {
                return RedirectToPage("/AccessDenied");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageFile != null)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var uploadPath = Path.Combine(_environment.WebRootPath, "Image", "news");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                News.Image = $"/Image/news/{fileName}";
            }

            var userJson = HttpContext.Session.GetString("UserSession");
            if (userJson != null)
            {
                var user = JsonSerializer.Deserialize<User>(userJson);
                News.AdminCreate = user?.Name;
            }
            News.CreateDate = DateTime.Now;

            await _newsService.AddAsync(News);

            return RedirectToPage("./Index");
        }
    }
}
