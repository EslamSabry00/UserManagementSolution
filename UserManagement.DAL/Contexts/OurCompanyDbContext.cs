using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.DAL.Models;

namespace UserManagement.DAL.Contexts
{
    public class OurCompanyDbContext : IdentityDbContext<ApplicationUser>
    {
        public OurCompanyDbContext(DbContextOptions<OurCompanyDbContext> options):base(options) { }

        //public CompanyDbContext()
        //{
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlServer("server = .; Database= CompanyDB; trusted_connection = true; MultipleActiveResultSets = true;");
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<IdentityUser>  Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }

    }
}
