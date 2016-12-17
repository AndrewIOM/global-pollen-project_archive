using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

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
            _context.Taxa.Add(entity);
        }

        public void Delete(Taxon entity)
        {
            _context.Taxa.Remove(entity);
        }

        public void Edit(Taxon entity)
        {
            _context.Taxa.Update(entity);
        }

        public PagedResult<Taxon> FindBy(Expression<Func<Taxon, bool>> predicate, int pageNumber, int pageSize)
        {
            var result = _context.Taxa
                .Include(m => m.ParentTaxon)
                    .ThenInclude(n => n.ParentTaxon)
                .Include(m => m.ChildTaxa)
                    .ThenInclude(x => x.ChildTaxa)
                .Where(predicate)
                .ToPagedList(pageNumber, pageSize);
            return result;
        }

        public Taxon FirstOrDefault(Expression<Func<Taxon, bool>> predicate)
        {
            var result = _context.Taxa
                .Include(m => m.ParentTaxon)
                    .ThenInclude(n => n.ParentTaxon)
                .Include(m => m.ChildTaxa)
                    .ThenInclude(x => x.ChildTaxa)
                .FirstOrDefault(predicate);
            return result;
        }

        public PagedResult<Taxon> GetAll(int pageNumber, int pageSize)
        {
            var result = _context.Taxa
                .Include(m => m.ParentTaxon)
                    .ThenInclude(n => n.ParentTaxon)
                .Include(m => m.ChildTaxa)
                    .ThenInclude(x => x.ChildTaxa)
                .ToPagedList(pageNumber, pageSize);
            return result;
        }
    }
}
