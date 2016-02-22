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
                //Grains
                .Include(f => f.ReferenceGrains)
                .Include(f => f.UserGrains)

                .Include(f => f.ChildTaxa)
                .ThenInclude(g => g.ReferenceGrains)

                .Include(f => f.ChildTaxa)
                .ThenInclude(g => g.UserGrains)

                .Include(f => f.ChildTaxa)
                .ThenInclude(g => g.ChildTaxa)
                .ThenInclude(s => s.ReferenceGrains)

                .Include(f => f.ChildTaxa)
                .ThenInclude(g => g.ChildTaxa)
                .ThenInclude(s => s.UserGrains)

                //Parent
                .Include(m => m.ParentTaxa)

                //Query
                .Where(where);
        }

        public IEnumerable<Taxon> GetAll()
        {
            return _context.Taxa
                .Include(m => m.ReferenceGrains)
                .ThenInclude(n => n.Images)
                .Include(m => m.UserGrains)
                .ThenInclude(n => n.Images)
                .Include(m => m.ChildTaxa)
                .ThenInclude(n => n.UserGrains)
                .Include(m => m.ChildTaxa)
                .ThenInclude(n => n.ReferenceGrains)
                .Include(m => m.ParentTaxa);
        }

        public Taxon GetById(int id)
        {
            return _context.Taxa
                .Include(m => m.ChildTaxa)
                .Include(m => m.ParentTaxa)
                .Include(m => m.ReferenceGrains)
                .ThenInclude(n => n.Images)
                .Include(m => m.UserGrains)
                .ThenInclude(n => n.Images)
                .FirstOrDefault(m => m.TaxonId == id);
        }
    }
}
