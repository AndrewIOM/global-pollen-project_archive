using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;

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
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(User entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
