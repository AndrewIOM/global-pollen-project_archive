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
    public class ReferenceGrainRepository : IRepository<ReferenceGrain>
    {
        private OxPollenDbContext _context;
        public ReferenceGrainRepository(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(ReferenceGrain entity)
        {
            _context.Add(entity);
        }

        public void Delete(ReferenceGrain entity)
        {
            throw new NotImplementedException();
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
                .FirstOrDefault(m => m.ReferenceGrainId == id);
        }

        public void Update(ReferenceGrain entity)
        {
            throw new NotImplementedException();
        }
    }
}
