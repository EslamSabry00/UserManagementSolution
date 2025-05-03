using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.DAL.Models;

namespace UserManagement.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository <Employee>
    {
        IQueryable<Employee> GetEmployeesByName(string name);
    }
}
