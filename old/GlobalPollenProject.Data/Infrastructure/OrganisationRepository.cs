using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class OrganisationRepository : IRepository<Organisation>
    {
        private PollenDbContext _context;
        public OrganisationRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(Organisation entity)
        {
            _context.Organisations.Add(entity);
        }

        public void Update(Organisation entity)
        {
            _context.Organisations.Update(entity);
        }

        public void Delete(Organisation entity)
        {
            _context.Organisations.Remove(entity);
        }

        public IEnumerable<Organisation> Find(Expression<Func<Organisation, bool>> where)
        {
            return _context.Organisations.Where(where);
        }

        public IEnumerable<Organisation> GetAll()
        {
            return _context.Organisations.Include(m => m.Members);
        }

        public Organisation GetById(int id)
        {
            return _context.Organisations.FirstOrDefault(m => m.OrganisationId == id);
        }
    }
}
