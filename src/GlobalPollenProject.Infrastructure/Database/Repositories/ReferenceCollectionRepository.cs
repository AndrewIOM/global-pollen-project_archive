using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class ReferenceCollectionRepository : IRepository<ReferenceCollection>
    {
        private PollenDbContext _context;
        public ReferenceCollectionRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(ReferenceCollection entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ReferenceCollection entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(ReferenceCollection entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ReferenceCollection> FindBy(Expression<Func<ReferenceCollection, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ReferenceCollection> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
