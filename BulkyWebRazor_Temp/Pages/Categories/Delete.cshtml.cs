using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Categories
{
	[BindProperties]
	public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db=db;
        }
        public Category category { get; set; }
        public void OnGet(int? id)
        {
			category = _db.categories.Find(id);
		}
        public IActionResult OnPost()
        {
            Category? obj = _db.categories.Find(category.Id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.categories.Remove(obj);
			_db.SaveChanges();
            TempData["success"] = "Category is Deleted successfully";
            return RedirectToPage("Index");
        
        }
    }
}
