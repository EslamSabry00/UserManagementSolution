using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UserManagement.BLL.Interfaces;
using UserManagement.BLL.Repositories;
using UserManagement.DAL.Models;

namespace UserManagement.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _DepartmentRepository;

        public DepartmentController(IDepartmentRepository DepartmentRepository)
        {
            _DepartmentRepository = DepartmentRepository;
        }
        public IActionResult Index()
        {
            var departments = _DepartmentRepository.GetAll();
            return View(departments);
        }
        public IActionResult Create() { 
            return View();
        }
        [HttpPost]
        public IActionResult create(Department department)
        {
            if (ModelState.IsValid)
            {
                int count = _DepartmentRepository.Add(department);
                if (count > 0)
                    TempData["Message"] = "Department Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        public IActionResult Details(int? id, string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();
            var department = _DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound();
            return View(viewName,department);
        }
        public IActionResult Edit(int? id) {
            return Details(id, "Edit");
            //if (id is null)
            //    return BadRequest();
            //var department = _DepartmentRepository.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit([FromRoute] int id,Department department) {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _DepartmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }
                
            }
            return View(department);
            
        }
        public IActionResult Delete(int id) {
            return Details(id, "Delete");
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department dep)
        {
            if (id != dep.Id)
                return BadRequest();
            try
            {
                _DepartmentRepository.Delete(dep);
                return RedirectToAction(nameof(Index));  
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dep);
            }
        }
    }
}
