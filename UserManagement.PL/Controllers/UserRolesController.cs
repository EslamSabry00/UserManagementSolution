using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Assign([FromRoute]string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleSelectionViewModel
                {
                    RoleName = role,
                    Selected = userRoles.Contains(role)
                }).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Assign(UserRolesViewModel model)
        {
            if (ModelState.IsValid) {
                var selectedRoles = model.Roles
                .Where(r => r.Selected)
                .Select(r => r.RoleName)
                .ToList();

                var unSelectedRoles = model.Roles
                .Where(r => !r.Selected)
                .Select(r => r.RoleName)
                .ToList();
                
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound();
                var userRoles = await _userManager.GetRolesAsync(user);

                // add Roles
                var rolesToAdd = selectedRoles.Except(userRoles);
                if (rolesToAdd.Any())
                {
                    var result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                }

                var rolesToRemove = userRoles.Except(selectedRoles);
                if (rolesToRemove.Any())
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                }
                
                return RedirectToAction("Index", "User");
            }
            return View(model);
        }
    }
}
