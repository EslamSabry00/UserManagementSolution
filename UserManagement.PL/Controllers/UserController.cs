using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DAL.Models;
using UserManagement.PL.Helpers;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper) {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string Email)
        {
            if (string.IsNullOrEmpty(Email)) {
                var Users =  _userManager.Users
                                        .Select(U => new UserViewModel()
                                        {
                                            Id = U.Id,
                                            UserName = U.UserName,
                                            Email = U.Email,
                                            PhoneNumber = U.PhoneNumber,
                                            Name = U.Name,
                                            Roles = _userManager.GetRolesAsync(U).Result
                                        });
                return View(Users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(Email);
                var MappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel>() { MappedUser });
            }
                
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();
            var MappedUser = _mapper.Map<ApplicationUser,UserViewModel>(user);
            //var MappedUser = new UserViewModel()
            //{
            //    Id = user.Id,  
            //    Email = user.Email,
            //    Name = user.Name,
            //    PhoneNumber = user.PhoneNumber,
            //    UserName = user.UserName,
            //    Roles = _userManager.GetRolesAsync(user).Result
            //};
            return View(viewName, MappedUser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.Name = UserVM.Name;
                    user.PhoneNumber = UserVM.PhoneNumber;
                    user.UserName = UserVM.UserName;
                    var Result =await _userManager.UpdateAsync(user);
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
            return View(UserVM);

        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] String id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(UserVM);
            }
        }
    }
}
