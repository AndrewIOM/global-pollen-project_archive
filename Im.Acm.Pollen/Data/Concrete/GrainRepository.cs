using Im.Acm.Pollen.Data.Abstract;
using Im.Acm.Pollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Im.Acm.Pollen.Data.Concrete
{
    public class GrainRepository : IGrainRepository
    {
        private PollenDbContext _context;
        public GrainRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(Grain entity)
        {
            _context.UserGrains.Add(entity);
        }

        public void Update(Grain entity)
        {
            _context.UserGrains.Update(entity);
        }

        public void Delete(Grain entity)
        {
            entity.IsDeleted = true;
            _context.UserGrains.Update(entity);
        }

        public IEnumerable<Grain> Find(Expression<Func<Grain, bool>> where)
        {
            var result = _context.UserGrains.Include(m => m.Identifications)
                .Include(m => m.IdentifiedAs)
                .ThenInclude(n => n.ParentTaxa)
                .ThenInclude(o => o.ParentTaxa)
                .Include(m => m.Images).Where(m => !m.IsDeleted).Where(where);
            return result;
        }

        public IEnumerable<Grain> GetAll()
        {
            return _context.UserGrains.Where(m => !m.IsDeleted).Include(m => m.Identifications)
                .Include(m => m.IdentifiedAs)
                .ThenInclude(n => n.ParentTaxa)
                .ThenInclude(o => o.ParentTaxa)
                .Include(m => m.Images);
        }

        public IEnumerable<Grain> GetAllDeleted()
        {
            return _context.UserGrains.Where(m => m.IsDeleted).Include(m => m.Identifications)
                .Include(m => m.IdentifiedAs)
                .ThenInclude(n => n.ParentTaxa)
                .ThenInclude(o => o.ParentTaxa)
                .Include(m => m.Images);
        }

        public Grain GetById(int id)
        {
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .Include(m => m.IdentifiedAs)
                .ThenInclude(n => n.ParentTaxa)
                .ThenInclude(o => o.ParentTaxa)
                .Include(m => m.Identifications)
                .ThenInclude(n => n.User)
                .Include(m => m.SubmittedBy)
                .Include(m => m.Images).ToList()
                .FirstOrDefault(m => m.Id == id);
            return result;
        }
    }
}
