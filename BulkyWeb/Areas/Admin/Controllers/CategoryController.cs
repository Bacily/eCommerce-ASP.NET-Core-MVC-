using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            //*Retrieve form database
            List<Category> objCategoryList = _UnitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "the Display Order can not match the Name.");
            }
            //if (obj.Name != null && obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "test error");
            //}
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category is created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            Category? CategoryFromDb1 = _UnitOfWork.Category.Get(u => u.Id == id);
            //Category? CategoryFromDb2 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? CategoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            return View(CategoryFromDb1);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category is edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            Category? CategoryFromDb1 = _UnitOfWork.Category.Get(u => u.Id == id);
            //Category? CategoryFromDb2 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? CategoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            return View(CategoryFromDb1);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _UnitOfWork.Category.Remove(obj);
            _UnitOfWork.Save();
            TempData["success"] = "Category is deleted successfully";
            return RedirectToAction("Index");
        }
    }
    //   public class CategoryController : Controller
    //   {
    //       private readonly ApplicationDbContext _db;
    //       public CategoryController(ApplicationDbContext db) 
    //       {
    //           _db = db;
    //       }
    //       public IActionResult Index()
    //       {
    //           //*Retrieve form database
    //           List<Category> objCategoryList = _db.Categories.ToList();
    //           return View(objCategoryList);
    //	}
    //	public IActionResult Create()
    //       {
    //           return View();
    //       }
    //       [HttpPost]
    //	public IActionResult Create(Category obj)
    //	{
    //           if (obj.Name == obj.DisplayOrder.ToString())
    //           {
    //               ModelState.AddModelError("Name", "the Display Order can not match the Name.");
    //           }
    //           //if (obj.Name != null && obj.Name.ToLower() == "test")
    //           //{
    //           //    ModelState.AddModelError("", "test error");
    //           //}
    //           if (ModelState.IsValid) 
    //           {
    //			_db.Categories.Add(obj);
    //			_db.SaveChanges();
    //			TempData["success"] = "Category is created successfully";
    //			return RedirectToAction("Index");
    //		}
    //		return View();
    //	}
    //	public IActionResult Edit(int? id)
    //	{
    //		Category? CategoryFromDb1 = _db.Categories.Find(id);
    //		//Category? CategoryFromDb2 = _db.Categories.FirstOrDefault(u=>u.Id==id);
    //		//Category? CategoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
    //		return View(CategoryFromDb1);
    //	}
    //	[HttpPost]
    //	public IActionResult Edit(Category obj)
    //	{
    //		if (ModelState.IsValid)
    //		{
    //			_db.Categories.Update(obj);
    //			_db.SaveChanges();
    //			TempData["success"] = "Category is edited successfully";
    //			return RedirectToAction("Index");
    //		}
    //		return View();
    //	}
    //	public IActionResult Delete(int? id)
    //	{
    //		Category? CategoryFromDb1 = _db.Categories.Find(id);
    //		//Category? CategoryFromDb2 = _db.Categories.FirstOrDefault(u=>u.Id==id);
    //		//Category? CategoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
    //		return View(CategoryFromDb1);
    //	}
    //	[HttpPost, ActionName("Delete")]
    //	public IActionResult DeletePost(int? id)
    //	{
    //		Category? obj = _db.Categories.Find(id);
    //		if (obj == null)
    //		{
    //			return NotFound();
    //		}
    //		_db.Categories.Remove(obj);
    //		_db.SaveChanges();
    //		TempData["success"] = "Category is deleted successfully";
    //		return RedirectToAction("Index");
    //	}
    //}
}
