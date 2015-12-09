using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using OxPollen.Models;
using OxPollen.Data.Abstract;

namespace OxPollen.Services.Concrete
{
    public class TaxonomyService : ITaxonomyService
    {
        private IRepository<Taxon> _taxonRepo;
        public TaxonomyService(IRepository<Taxon> taxonRepo)
        {
            _taxonRepo = taxonRepo;
        }

        public IEnumerable<Taxon> GetAll()
        {
            var result = _taxonRepo.GetAll();
            return result;
        }

        public IEnumerable<Taxon> GetAll(Taxonomy rank)
        {
            var result = _taxonRepo.Find(m => m.Rank == rank);
            return result;
        }
    }
}
