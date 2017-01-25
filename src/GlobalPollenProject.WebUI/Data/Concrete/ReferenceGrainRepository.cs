using GlobalPollenProject.WebUI.Data.Abstract;
using GlobalPollenProject.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.WebUI.Data.Concrete
{
    public class ReferenceGrainRepository : IRepository<ReferenceGrain>
    {
        private PollenDbContext _context;
        public ReferenceGrainRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(ReferenceGrain entity)
        {
            _context.Add(entity);
        }

        public void Delete(ReferenceGrain entity)
        {
            _context.ReferenceGrains.Remove(entity);
        }

        public IEnumerable<ReferenceGrain> Find(Expression<Func<ReferenceGrain, bool>> where)
        {
            return _context.ReferenceGrains.Include(m => m.Images).Where(where);
        }

        public IEnumerable<ReferenceGrain> GetAll()
        {
            return _context.ReferenceGrains.Include(m => m.Images);
        }

        public ReferenceGrain GetById(int id)
        {
            return _context.ReferenceGrains.Include(m => m.Images)
                .Include(m => m.Collection)
                .ThenInclude(n => n.User)
                .Include(m => m.Taxon)
                .ThenInclude(n => n.ParentTaxa)
                .ThenInclude(o => o.ParentTaxa)
                .FirstOrDefault(m => m.ReferenceGrainId == id);
        }

        public void Update(ReferenceGrain entity)
        {
            _context.ReferenceGrains.Update(entity);
        }
    }
}
