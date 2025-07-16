using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.PL.ViewModels
{
    public class RoleViewModel : IdentityRole
    {
        public RoleViewModel() { 
            Id = Guid.NewGuid().ToString();
        }
        public string Id {  get; set; }
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
