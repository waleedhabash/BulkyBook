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

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _context;
        

        public CompanyController(IUnitOfWork context)
        {
            _context = context;
          
        }

        public IActionResult Index()
        {
           
            return View();
        }

       
        public IActionResult Upsert(int? Id)
        {
            Company company = new();
            
            if (Id == null || Id == 0)
            {
                //create
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            {
                //update
                company = _context.Company.GetFirstOrDefault(s => s.Id == Id);
                return View(company);
                
            }
           
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj,IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                
                if (obj.Id==0)
                {
                    _context.Company.Add(obj);
                }
                else
                {
                    _context.Company.Update(obj);
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
            var companyList = _context.Company.GetAll();
            return Json(new {data=companyList});
        }
        [HttpDelete]
       
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _context.Company.GetFirstOrDefault(n => n.Id == Id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            
                
           
            _context.Company.Remove(obj);
            _context.Save();
            return Json(new { success = true, message = "Delete Successful" });
           
        }
        #endregion
    }


}
