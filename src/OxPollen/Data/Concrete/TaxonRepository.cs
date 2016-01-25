using OxPollen.Data.Abstract;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Data.Entity;

namespace OxPollen.Data.Concrete
{
    public class TaxonRepository : IRepository<Taxon>
    {
        private OxPollenDbContext _context;
        public TaxonRepository(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(Taxon entity)
        {
            _context.Taxa.Add(entity);
        }

        public void Update(Taxon entity)
        {
            _context.Taxa.Update(entity);
        }

        public void Delete(Taxon entity)
        {
            _context.Taxa.Remove(entity);
        }

        public IEnumerable<Taxon> Find(Expression<Func<Taxon, bool>> where)
        {
            return _context.Taxa
                .Include(m => m.ChildTaxa)
                .Where(where);
        }

        public IEnumerable<Taxon> GetAll()
        {
            return _context.Taxa
                .Include(m => m.ChildTaxa);
        }

        public Taxon GetById(int id)
        {
            return _context.Taxa
                .Include(m => m.ChildTaxa)
                .Include(m => m.ParentTaxa)
                .FirstOrDefault(m => m.TaxonId == id);
        }
    }
}
