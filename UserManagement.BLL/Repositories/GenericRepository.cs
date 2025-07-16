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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected OurCompanyDbContext _dbContext;

        public GenericRepository(OurCompanyDbContext dbContext) // request creation of object from clr
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            //return _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            if (typeof(T) == typeof(Employee))
                return await _dbContext.Employees.Include(e=>e.Department).Where(e => e.Id == id).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(id);
        } 

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).ToListAsync();
            }
            else
                return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<int> Update(T item)
        {
            _dbContext.Set<T>().Update(item);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
