using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class KewBackboneTaxonRepository : IRepository<BackboneTaxonAggregate>
    {
        private PollenDbContext _context;
        public KewBackboneTaxonRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(BackboneTaxonAggregate entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BackboneTaxonAggregate entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(BackboneTaxonAggregate entity)
        {
            throw new NotImplementedException();
        }

        public PagedResult<BackboneTaxonAggregate> FindBy(Expression<Func<BackboneTaxonAggregate, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.BackboneTaxa
                .Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa).ThenInclude(o => o.ChildTaxa)
                .Where(predicate).ToPagedList(pageNumber, pageSize);
        }

        public BackboneTaxonAggregate FirstOrDefault(Expression<Func<BackboneTaxonAggregate, bool>> predicate)
        {
            return _context.BackboneTaxa
                .Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa).ThenInclude(o => o.ChildTaxa)
                .FirstOrDefault(predicate);
        }

        public PagedResult<BackboneTaxonAggregate> GetAll(int pageNumber, int pageSize)
        {
            return _context.BackboneTaxa
                .Include(m => m.ParentTaxa)
                    .ThenInclude(n => n.ParentTaxa)
                .Include(m => m.ChildTaxa)
                    .ThenInclude(o => o.ChildTaxa)
                .ToPagedList(pageNumber, pageSize);
        }
    }
}
