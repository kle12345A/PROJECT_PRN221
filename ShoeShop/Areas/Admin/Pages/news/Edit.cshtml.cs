using System;
using System.IO;
using System.Threading.Tasks;
using BusinessObject.news;
using DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoeShop.Areas.Admin.Pages.news
{
    public class EditModel : PageModel
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _environment;

        public EditModel(INewsService newsService, IWebHostEnvironment environment)
        {
            _newsService = newsService;
            _environment = environment;
        }

        [BindProperty]
        public News News { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetByIdAsync(id.Value);
            if (news == null)
            {
                return NotFound();
            }

            News = news;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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

                // Lưu đường dẫn ảnh vào thuộc tính của News
                News.Image = $"/Image/news/{fileName}";
            }
			else
			{
				News.Image = Request.Form["CurrentImage"];
			}
			var userJson = HttpContext.Session.GetString("UserSession");
            if (userJson != null)
            {
                var userElement = System.Text.Json.JsonDocument.Parse(userJson).RootElement;

                // Truy xuất giá trị của 'Name' từ JsonElement
                var userName = userElement.GetProperty("Name").GetString();
                News.AdminUpdate = userName;
            }
            News.UpdateDate = DateTime.Now;

            try
            {
                await _newsService.UpdateAsync(News);
            }
            catch (Exception)
            {
                if (!await NewsExists(News.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> NewsExists(int id)
        {
            return (await _newsService.GetByIdAsync(id)) != null;
        }
    }
}
