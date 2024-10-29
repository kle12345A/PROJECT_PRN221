using BusinessObject.contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using DataAccess.Models;

namespace ShoeShop.Pages.contact
{
    public class ContactModel : PageModel
    {
        private readonly IContactService _contactService;

        public ContactModel(IContactService contactService)
        {
            _contactService = contactService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(IFormCollection form)
        {
            var name = form["name"];
            var email = form["email"];
            var phone = form["phone"];
            var address = form["address"];
            var content = form["content"];

            var newContact = new Contact
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                Content = content,
                CreateDate = DateTime.Now,
                Status = true
            };

            var result = _contactService.AddAsync(newContact);

            if (result != null)
            {
                TempData["SuccessMessage"] = "Your contact has been successfully submitted.";
            }
            else
            {
                TempData["ErrorMessage"] = "There was an error submitting your contact. Please try again.";
            }

            return RedirectToPage("Index");
        }

    }
}
