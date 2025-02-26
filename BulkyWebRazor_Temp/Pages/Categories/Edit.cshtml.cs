using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		public Category category { get; set; }
		public EditModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int? id)
		{
			if(id!=null && id != 0)
			{
				category = _db.categories.Find(id);
			}

		}
		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				_db.categories.Update(category);
				_db.SaveChanges();
				TempData["success"] = "Category is edited successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}
