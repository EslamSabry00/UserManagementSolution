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

        public int Add(T item)
        {
            _dbContext.Set<T>().Add(item);
            return _dbContext.SaveChanges();
        }

        public int Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
            return _dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            //return _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            if (typeof(T) == typeof(Employee))
                return _dbContext.Employees.Include(e=>e.Department).Where(e => e.Id == id).FirstOrDefault() as T;
            return _dbContext.Set<T>().Find(id);
        } 

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).ToList();
            }
            else
                return _dbContext.Set<T>().ToList();
        }

        public int Update(T item)
        {
            _dbContext.Set<T>().Update(item);
            return _dbContext.SaveChanges();
        }
    }
}
