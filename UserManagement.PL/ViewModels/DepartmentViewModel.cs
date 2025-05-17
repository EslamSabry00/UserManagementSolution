using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using UserManagement.DAL.Models;

namespace UserManagement.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "code is required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "code is required")]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        //navigational prop[Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
