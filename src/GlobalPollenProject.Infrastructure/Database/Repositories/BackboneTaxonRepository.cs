using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class KewBackboneTaxonRepository : IRepository<KewBackboneTaxon>
    {
        private PollenDbContext _context;
        public KewBackboneTaxonRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(KewBackboneTaxon entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(KewBackboneTaxon entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(KewBackboneTaxon entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<KewBackboneTaxon> FindBy(Expression<Func<KewBackboneTaxon, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<KewBackboneTaxon> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
