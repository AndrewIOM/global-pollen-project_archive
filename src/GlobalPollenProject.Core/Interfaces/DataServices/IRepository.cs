using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> where);

        // other data access methods could also be included.

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
