using BasicCRUD.Models;
using BasicCRUD.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        readonly private ApplicationDbContext _db;
        readonly private IWebHostEnvironment _environment;

        public EmployeeController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var employees = _db.Employees.Include(X => X.Department).ToList();
            return View( employees);
        } 

        public IActionResult Create()
        {
            List<Department> departments = _db.Departments.ToList();
            ViewBag.Dept = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var exist = _db.Employees.Find(employee.EmpID);
                if (exist != null)
                {
                    //if we use first argument empty the error massage will show into "asp-validation-summary=\"All\""
                    //ModelState.AddModelError("", $"{exist.EmpID} already exist.");
                    ModelState.AddModelError("EmpID", $"{exist.EmpID} already exist."); //It will show below the input field.
                }
                else
                {
                    //If We use ViewModel object in View then the data need to put into the main Employee object
                    Employee mainEmployee = new Employee()
                    {
                        EmpID = employee.EmpID,
                        Name = employee.Name,
                        Gender = employee.Gender,
                        Phone = employee.Phone,
                        Email = employee.Email,
                        JoinDate = employee.JoinDate,
                        ProPic = "Pictures/default.jpg",
                        DeptId = employee.DeptId
                    };

                    if (employee.ProPic != null)
                    {
                        //Path of project.
                        var rootPath = _environment.WebRootPath;

                        //Path of inside of the project folder(Where pic will store.) with file name(That get from view.) This path will assign to ProPic prop of main employee obj. Note : Guid used that every path must be unique;
                        var projFilePath = "Pictures/" + Guid.NewGuid() + employee.ProPic.FileName;
                        mainEmployee.ProPic = projFilePath;

                        //fullPath will use in FileStream to copy the picture.png to the project from any location.
                        var fullPath = Path.Combine(rootPath, projFilePath);
                        FileStream stream = new FileStream(fullPath, FileMode.Create); //create a FileStream Obj that help to store any file into the given path(in this case fullPath)

                        employee.ProPic.CopyTo(stream); //copy the picture(comes form view) to "Pictures" folder 
                    }
                    _db.Employees.Add(mainEmployee);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            List<Department> departments = _db.Departments.ToList();
            ViewBag.Dept = departments;
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(string EmpId)
        {
            var existEmployee = _db.Employees.Find(EmpId);
            ViewBag.CurrProPic = existEmployee.ProPic;
            EmployeeViewModel employeeViewModel = new EmployeeViewModel() { 
                EmpID = existEmployee.EmpID,
                Name = existEmployee.Name,
                Gender = existEmployee.Gender,
                Phone = existEmployee.Phone,
                Email = existEmployee.Email,
                JoinDate = existEmployee.JoinDate
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employee)
        {
            var existingEmployee =  _db.Employees.Find(employee.EmpID);
            ViewBag.CurrProPic = existingEmployee.ProPic;
            if (ModelState.IsValid)
            {
                existingEmployee.EmpID = employee.EmpID;
                existingEmployee.Name = employee.Name;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.Email = employee.Email;
                existingEmployee.JoinDate = employee.JoinDate;

                if (employee.ProPic != null)
                {
                    var rootPath = _environment.WebRootPath;
                    var projFolderPath = "Pictures/" + Guid.NewGuid() + employee.ProPic.FileName;
                    existingEmployee.ProPic = projFolderPath;
                    var fullPath = Path.Combine(rootPath, projFolderPath);
                    FileStream stream = new FileStream(fullPath, FileMode.Create);
                    employee.ProPic.CopyTo(stream);
                }
                _db.Employees.Update(existingEmployee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        /*
         Note : Into Index view the button "Delete" has a attribut "asp-rout-EmpId" and the action parameter name "(string EmpId)" must be same. Otherwise we can not catch the ID of the record.
         */

        public IActionResult Delete(string EmpId)
        {
            var employee = _db.Employees.Find(EmpId);
            return View(employee);
        }

        [HttpPost]
        //[ActionName("Delete")]
        public IActionResult DeleteDone(string EmpId)
        {
            var employee = _db.Employees.Find(EmpId);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
