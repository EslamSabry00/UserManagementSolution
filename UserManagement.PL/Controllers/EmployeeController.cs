using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserManagement.BLL.Interfaces;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        public readonly IMapper _Mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _Mapper = mapper;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> Employees;
            if (SearchValue == null)
                Employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                Employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            var mappedEmp = _Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
            return View(mappedEmp);
        }
        public IActionResult Create() {
            //ViewBag.Departments = _departmentRepository.GetAll(); 
            return View();
        }
        [HttpPost]
        public IActionResult create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                ///Manual Mapping
                ///var Employee = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Address = employeeVM.Address,
                ///    Email = employeeVM.Email,
                ///    Salary = employeeVM.Salary,
                ///    Age = employeeVM.Age,
                ///    DeptId = employeeVM.DeptId,
                ///    IsActive = employeeVM.IsActive,
                ///    HireDate = employeeVM.HireDate,
                ///    Phone= employeeVM.Phone
                ///};
                var mappedEmp = _Mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                return RedirectToAction(nameof(Index));
            } 
            return View(employeeVM);
        }
        public IActionResult Details(int? id, string viewName = "Details" )
        {
            if (id is null)
                return BadRequest();
            var Employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            if (Employee is null)
                return NotFound();
            var mappedEmp = _Mapper.Map<Employee, EmployeeViewModel >(Employee);
            return View(viewName, mappedEmp);
        }
        public IActionResult Edit(int? id) {
            return Details(id, "Edit");
            //if (id is null)
            //    return BadRequest();
            //var Employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            //if (Employee is null)
            //    return NotFound();
            //return View(Employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Edit([FromRoute] int id,EmployeeViewModel EmployeeVM) {
            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _Mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }
                
            }
            return View(EmployeeVM);
            
        }
        public IActionResult Delete(int id) {
            return Details(id, "Delete");
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel empVM)
        {
            if (id != empVM.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _Mapper.Map<EmployeeViewModel, Employee>(empVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                return RedirectToAction(nameof(Index));  
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(empVM);
            }
        }
    }
}
