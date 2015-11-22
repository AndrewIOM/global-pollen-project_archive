using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxPollen.Models;

namespace OxPollen.Services.Concrete
{
    public class TaxonomyService : ITaxonomyService
    {
        private OxPollenDbContext _context; 
        public TaxonomyService(OxPollenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Taxon> GetAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Taxon> ITaxonomyService.GetAllFamilies()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Taxon> ITaxonomyService.GetAllGenus()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Taxon> ITaxonomyService.GetAllSpecies()
        {
            throw new NotImplementedException();
        }
    }
}
