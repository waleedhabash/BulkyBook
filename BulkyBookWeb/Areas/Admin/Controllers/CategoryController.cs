using BulkBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _context;
        public CategoryController(IUnitOfWork context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cats = _context.Category.GetAll();
            return View(cats);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name.ToString() == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder Cannot Be Like Name");
            }
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                _context.Save();
                TempData["success"] = "Data Have Been Added";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var category = _context.Category.GetFirstOrDefault(n => n.Id == Id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name.ToString() == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder Cannot Be Like Name");
            }
            if (ModelState.IsValid)
            {
                _context.Category.Update(category);
                _context.Save();
                TempData["success"] = "Data Have Been Updated";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var category = _context.Category.GetFirstOrDefault(n => n.Id == Id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category category)
        {
            _context.Category.Remove(category);
            _context.Save();
            TempData["success"] = "Data Have Been Deleted";
            return RedirectToAction("Index");

        }
    }
}
