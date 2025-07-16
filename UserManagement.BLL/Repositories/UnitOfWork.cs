using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.BLL.Interfaces;
using UserManagement.DAL.Contexts;

namespace UserManagement.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly OurCompanyDbContext _dbContext;

        public IDepartmentRepository DepartmentRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }

        public UnitOfWork(OurCompanyDbContext dbContext)
        {
            DepartmentRepository = new DepartmentRepository(dbContext);
            EmployeeRepository = new EmployeeRepository(dbContext);
            _dbContext = dbContext;
        }

        async Task<int> IUnitOfWork.Complete()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
            => _dbContext.Dispose();
    }
}
