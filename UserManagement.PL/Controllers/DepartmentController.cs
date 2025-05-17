using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UserManagement.BLL.Interfaces;
using UserManagement.BLL.Repositories;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _DepartmentRepository;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_DepartmentRepository = DepartmentRepository;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var mapped = _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(mapped);
        }
        public IActionResult Create() { 
            return View();
        }
        [HttpPost]
        public IActionResult create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                int count = _unitOfWork.DepartmentRepository.Add(mapped);
                if (count > 0)
                    TempData["Message"] = "Department Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public IActionResult Details(int? id, string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            var mapped = _mapper.Map<Department,DepartmentViewModel>(department);
            if (department is null)
                return NotFound();
            return View(viewName, mapped);
        }
        public IActionResult Edit(int? id) {
            return Details(id, "Edit");
            //if (id is null)
            //    return BadRequest();
            //var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit([FromRoute] int id,DepartmentViewModel departmentVM) {
            if (id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(mapped);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }
                
            }
            return View(departmentVM);
            
        }
        public IActionResult Delete(int id) {
            return Details(id, "Delete");
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, DepartmentViewModel depVM)
        {
            if (id != depVM.Id)
                return BadRequest();
            try
            {
                var mapped = _mapper.Map<DepartmentViewModel,Department>(depVM);
                _unitOfWork.DepartmentRepository.Delete(mapped);
                return RedirectToAction(nameof(Index));  
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(depVM);
            }
        }
    }
}
