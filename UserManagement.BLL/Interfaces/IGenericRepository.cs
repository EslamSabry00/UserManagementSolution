﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.DAL.Models;

namespace UserManagement.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        int Add(T item);

        int Update(T item);

        int Delete(T item);
    }
}
