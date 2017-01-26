using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GlobalPollenProject.WebUI.Data.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> where);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
