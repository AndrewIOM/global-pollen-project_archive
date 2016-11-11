using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class TaxonRepository : IRepository<Taxon>
    {
        private PollenDbContext _context;
        public TaxonRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(Taxon entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Taxon entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Taxon entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Taxon> FindBy(Expression<Func<Taxon, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Taxon> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
