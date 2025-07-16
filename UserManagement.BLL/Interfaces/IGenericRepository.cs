using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.DAL.Models;

namespace UserManagement.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
       Task<IEnumerable<T>> GetAll();

       Task<T> Get(int id);

       Task<int> Add(T item);

       Task<int> Update(T item);

       Task<int> Delete(T item);
    }
}
