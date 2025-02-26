using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]// to ensure that the form data is mapped to category
        public Category category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            _db.categories.Add(category);
            _db.SaveChanges();
            TempData["success"] = "Category is Created successfully";
            return RedirectToPage("Index");
        }
    }
}
