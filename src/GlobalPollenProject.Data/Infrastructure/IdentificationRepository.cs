using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Data.Interfaces;
using GlobalPollenProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class IdentificationRepository : IRepository<Identification>
    {
        private readonly PollenDbContext _context;
        public IdentificationRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(Identification entity)
        {
            _context.Identifications.Add(entity);
        }

        public void Delete(Identification entity)
        {
            _context.Identifications.Remove(entity);
        }

        public IEnumerable<Identification> Find(Expression<Func<Identification, bool>> where)
        {
            return _context.Identifications
                .Include(m => m.User)
                .Include(m => m.Grain)
                .Where(where);
        }

        public IEnumerable<Identification> GetAll()
        {
            return _context.Identifications;
        }

        public Identification GetById(int id)
        {
            return _context.Identifications
                .Include(m => m.User)
                .Include(m => m.Grain)
                .FirstOrDefault(m => m.IdentificationId == id);
        }

        public void Update(Identification entity)
        {
            _context.Identifications.Update(entity);
        }
    }
}
