using BasicCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCRUD.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //IEnumerable<Department> departments = _db.Departments.ToList();
            var departments = from department in _db.Departments select department;
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
           
        }


        [HttpPost]
        public IActionResult Create(Department department) 
        {
            if(ModelState.IsValid)
            {
                _db.Departments.Add(department);
                _db.SaveChanges();
                //return RedirectToAction("Index");
                return PartialView("_Result", true);
            }
            //return View(department);s
            return PartialView("_Result", false);
        }

        [HttpGet]
        public IActionResult Edit(int DeptId)
        {
            var department = _db.Departments.Find(DeptId);
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            if(ModelState.IsValid)
            {
                _db.Departments.Update(department);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int DeptId)
        {
            var department = _db.Departments.Find(DeptId);
            return View(department);
        }

        [HttpPost]
        //here We catch hole object form view and overload Delete Method by parameter change.
        //instead we can catch only Id of department in this case both delete method accepte DeptId. So, They match each other which is a error. In this situation we have to change method name like HttpGet = "Delete(int DeptId)" and HttpPost = "DeleteDone(int DeptId)" 
        public IActionResult Delete(Department department)
        {
            _db.Departments.Remove(department);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
