using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.BLL.Interfaces;
using UserManagement.DAL.Contexts;
using UserManagement.DAL.Models;

namespace UserManagement.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(OurCompanyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
