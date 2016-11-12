using System;
using System.Linq.Expressions;
using GlobalPollenProject.Core.Extensions;

public interface IRepository<T> where T : class {
    
    PagedResult<T> GetAll(int pageNumber, int pageSize);
    PagedResult<T> FindBy(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
    T FirstOrDefault(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Delete(T entity);
    void Edit(T entity);
}
