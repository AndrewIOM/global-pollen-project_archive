using GlobalPollenProject.Data.Interfaces;
using GlobalPollenProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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
            _context.Add(entity);
        }

        public void Delete(ReferenceCollection entity)
        {
            _context.Remove(entity);
        }

        public IEnumerable<ReferenceCollection> Find(Expression<Func<ReferenceCollection, bool>> where)
        {
            return _context.ReferenceCollections.Include(m => m.User).Where(where);
        }

        public IEnumerable<ReferenceCollection> GetAll()
        {
            return _context.ReferenceCollections.Include(m => m.User).Include(m => m.Grains);
        }

        public ReferenceCollection GetById(int id)
        {
            return _context.ReferenceCollections
                .Include(m => m.User)
                .Include(m => m.Grains)
                .ThenInclude(m => m.Taxon)
                .ThenInclude(m => m.ParentTaxa)
                .ThenInclude(m => m.ParentTaxa)
                .Include(m => m.Grains)
                .ThenInclude(n => n.Images)
                .FirstOrDefault(i => i.Id == id);
        }

        public void Update(ReferenceCollection entity)
        {
            _context.ReferenceCollections.Update(entity);
        }
    }
}
