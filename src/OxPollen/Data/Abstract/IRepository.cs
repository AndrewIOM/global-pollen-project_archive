using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OxPollen.Data.Abstract
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
