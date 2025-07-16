using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;
using System.Linq;
using AutoMapper;
using UserManagement.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper) {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string name)
        {
            
            if (string.IsNullOrEmpty(name))
            {
                var roles = _roleManager.Roles
                                        .Select(R => new RoleViewModel()
                                        {
                                            Id = R.Id,
                                            Name = R.Name,
                                        });
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                var MappedRole = _mapper.Map<IdentityRole,RoleViewModel>(role);
                if (role != null) { 
                    return View(new List<RoleViewModel>() { MappedRole });
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Role Not Found");
                    return View(Enumerable.Empty<RoleViewModel>());
                }
                
            }

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel RoleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(RoleVM);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(RoleVM);

        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();
            var MappedRole = _mapper.Map<IdentityRole,RoleViewModel>(role);
            return View(viewName, MappedRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel RoleVM)
        {
            if (id != RoleVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = RoleVM.Name;
                    var Result = await _roleManager.UpdateAsync(role);
                    if (Result.Succeeded)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (var error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);

                        }
                    }

                }
                catch (Exception EX)
                {
                    ModelState.AddModelError(string.Empty, EX.Message);
                }

            }
            return View(RoleVM);

        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] String id, RoleViewModel RoleVM)
        {
            if (id != RoleVM.Id)
                return BadRequest();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(RoleVM);
            }
        }
    }
}
