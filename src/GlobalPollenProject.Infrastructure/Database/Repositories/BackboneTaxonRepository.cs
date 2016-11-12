using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

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

        public PagedResult<KewBackboneTaxon> FindBy(Expression<Func<KewBackboneTaxon, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.BackboneTaxa
                .Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa).ThenInclude(o => o.ChildTaxa)
                .Where(predicate).ToPagedList(pageNumber, pageSize);
        }

        public KewBackboneTaxon FirstOrDefault(Expression<Func<KewBackboneTaxon, bool>> predicate)
        {
            return _context.BackboneTaxa
                .Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa).ThenInclude(o => o.ChildTaxa)
                .FirstOrDefault(predicate);
        }

        public PagedResult<KewBackboneTaxon> GetAll(int pageNumber, int pageSize)
        {
            return _context.BackboneTaxa.Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa).ThenInclude(o => o.ChildTaxa).ToPagedList(pageNumber, pageSize);
        }
    }
}
