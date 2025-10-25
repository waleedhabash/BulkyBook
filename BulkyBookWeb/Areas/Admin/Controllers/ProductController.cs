using BulkBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
           
            return View();
        }

       
        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _context.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _context.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (Id == null || Id == 0)
            {
                //create
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _context.Product.GetFirstOrDefault(s => s.Id == Id);
                return View(productVM);
                
            }
           
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string wwwRotePath=_webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRotePath, @"Images\products");
                    var extention = Path.GetExtension(file.FileName);
                    if (obj.Product.ImageUrl!=null)
                    {
                        var oldImagePath = Path.Combine(wwwRotePath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extention;
                }
                if (obj.Product.Id==0)
                {
                    _context.Product.Add(obj.Product);
                }
                else
                {
                    _context.Product.Update(obj.Product);
                }
               
                _context.Save();
                TempData["success"] = "Data Have Been Saved";
                return RedirectToAction("Index");
            }
            return View();
        }
        
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _context.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new {data=productList});
        }
        [HttpDelete]
       
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _context.Product.GetFirstOrDefault(n => n.Id == Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
           
            _context.Product.Remove(obj);
            _context.Save();
            return Json(new { success = true, message = "Delete Successful" });
           
        }
        #endregion
    }


}
