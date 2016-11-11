using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class BackboneTaxonRepository : IRepository<PlantListTaxon>
    {
        private PollenDbContext _context;
        public BackboneTaxonRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(PlantListTaxon entity)
        {
            _context.PlantListTaxa.Add(entity);
        }

        public void Update(PlantListTaxon entity)
        {
            _context.PlantListTaxa.Update(entity);
        }

        public void Delete(PlantListTaxon entity)
        {
            _context.PlantListTaxa.Remove(entity);
        }

        public IEnumerable<PlantListTaxon> Find(Expression<Func<PlantListTaxon, bool>> where)
        {
            return _context.PlantListTaxa.Where(where);
        }

        public IEnumerable<PlantListTaxon> GetAll()
        {
            return _context.PlantListTaxa
                .Include(m => m.ParentTaxa);
        }

        public PlantListTaxon GetById(int id)
        {
            return _context.PlantListTaxa
                .FirstOrDefault(m => m.Id == id);
        }
    }
}
