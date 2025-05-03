using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.BLL.Interfaces;
using UserManagement.DAL.Contexts;
using UserManagement.DAL.Models;

namespace UserManagement.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly OurCompanyDbContext _dbContext;
        public EmployeeRepository(OurCompanyDbContext dbContext) : base(dbContext) { 
            _dbContext = dbContext;
        }

        IQueryable<Employee> IEmployeeRepository.GetEmployeesByName(string name)
        {
            return _dbContext.Employees.Where(E => E.Name.Contains(name));
        }
    }
}

