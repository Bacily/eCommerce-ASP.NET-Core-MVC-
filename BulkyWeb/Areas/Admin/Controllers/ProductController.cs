using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties:"Category").ToList();
           
            return View(objProductList);
        }
		//     public IActionResult Create()
		//     {
		////IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category
		////	.GetAll().Select(u => new SelectListItem
		////	{
		////		Text = u.Name,
		////		Value = u.Id.ToString()
		////	});
		////ViewBag.CategoryList = CategoryList;
		////ViewData["CategoryList"] = CategoryList;
		//ProductVM productVM = new()
		//{
		//	CategoryList = _UnitOfWork.Category
		//	.GetAll().Select(u => new SelectListItem
		//	{
		//		Text = u.Name,
		//		Value = u.Id.ToString()
		//	}),
		//	Product = new Product()
		//};
		//return View(productVM);
		//     }
		//[HttpPost]
		//public IActionResult Create(ProductVM productVM)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_UnitOfWork.Product.Add(productVM.Product);
		//		_UnitOfWork.Save();
		//		return RedirectToAction("Index");
		//	}
		//	else
		//	{
		//		productVM.CategoryList = _UnitOfWork.Category
		//		.GetAll().Select(u => new SelectListItem
		//		{
		//			Text = u.Name,
		//			Value = u.Id.ToString()
		//		});
		//		return View(productVM);
		//	}
		//}
		public IActionResult Upsert(int? id) //updateinsert
		{
			ProductVM productVM = new()
			{
				CategoryList = _UnitOfWork.Category
				.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				Product = new Product()
			};
            if(id == null|| id == 0)
            {
                //create
				return View(productVM);

			}
            else
            {
                //update
                productVM.Product = _UnitOfWork.Product.Get(u=>u.Id== id);
				return View(productVM);
			}
		}
		[HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid) 
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename  = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwRootPath, @"images\product");
                    Console.WriteLine($"Producr Path: {productpath}");
                    Console.WriteLine(Directory.Exists(productpath));
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    
                    productVM.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }
                _UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _UnitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
				return View(productVM);
			}
        }
        //public IActionResult Edit(int? id) 
        //{ 
        //    Product? product = _UnitOfWork.Product.Get(u  => u.Id == id);
        //    return View(product);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product product) 
        //{
        //    if (ModelState.IsValid) 
        //    {
        //        _UnitOfWork.Product.Update(product);
        //        _UnitOfWork.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        //public IActionResult Delete(int? id) 
        //{ 
        //    Product? product = _UnitOfWork.Product.Get(u => u.Id == id);
        //    return View(product);
        //}
        //[HttpPost]
        //public IActionResult Delete(Product product) 
        //{
        //    if(product == null)
        //    {
        //        return NotFound();
        //    }

        //    _UnitOfWork.Product.Remove(product);
        //    _UnitOfWork.Save();
        //    return RedirectToAction("Index");
        //}
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

		[HttpDelete]
		public IActionResult Delete(int ? id)
		{
            var productTOBeDeleted = _UnitOfWork.Product.Get(u => u.Id == id);
            if(productTOBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
			var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath, productTOBeDeleted.ImageUrl.TrimStart('\\'));
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}
            _UnitOfWork.Product.Remove(productTOBeDeleted);
            _UnitOfWork.Save();
			List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { success = false, message = "Delete Successful" });
		}

		#endregion
	}
}
