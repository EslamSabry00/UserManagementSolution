using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.BLL.Interfaces;
using UserManagement.DAL.Models;

namespace UserManagement.PL.Controllers
{
    public class EmployeeController : Controller
    {
       private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            var Employees = _employeeRepository.GetAll();
            return View(Employees);
        }
        public IActionResult Create() {
            ViewBag.Departments = _departmentRepository.GetAll(); 
            return View();
        }
        [HttpPost]
        public IActionResult create(Employee Employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }
        public IActionResult Details(int? id, string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();
            var Employee = _employeeRepository.Get(id.Value);

            if (Employee is null)
                return NotFound();
            return View(viewName,Employee);
        }
        public IActionResult Edit(int? id) {
            ViewBag.Departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
            //if (id is null)
            //    return BadRequest();
            //var Employee = _EmployeeRepository.Get(id.Value);
            //if (Employee is null)
            //    return NotFound();
            //return View(Employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit([FromRoute] int id,Employee Employee) {
            if (id != Employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(Employee);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }
                
            }
            return View(Employee);
            
        }
        public IActionResult Delete(int id) {
            return Details(id, "Delete");
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee emp)
        {
            if (id != emp.Id)
                return BadRequest();
            try
            {
                _employeeRepository.Delete(emp);
                return RedirectToAction(nameof(Index));  
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(emp);
            }
        }
    }
}
