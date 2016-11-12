using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class UserRepository : IRepository<User>
    {
        private PollenDbContext _context;
        public UserRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Add(entity);
        }

        public void Delete(User entity)
        {
            _context.Remove(entity);
        }

        public void Edit(User entity)
        {
            _context.Update(entity);
        }

        public PagedResult<User> FindBy(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.Users.Include(m => m.Organisation).Where(predicate).ToPagedList(pageNumber, pageSize);
        }

        public User FirstOrDefault(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Include(m => m.Organisation).FirstOrDefault(predicate);
        }

        public PagedResult<User> GetAll(int pageNumber, int pageSize)
        {
            return _context.Users.Include(m => m.Organisation).ToPagedList(pageNumber, pageSize);
        }
    }
}
