using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class UnknownGrainRepository : IRepository<UnknownGrain>
    {
        private PollenDbContext _context;
        public UnknownGrainRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(UnknownGrain entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UnknownGrain entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(UnknownGrain entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UnknownGrain> FindBy(Expression<Func<UnknownGrain, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UnknownGrain> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
