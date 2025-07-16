using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAgree { get; set; }
        public string Name { get; set; }
    }
}
