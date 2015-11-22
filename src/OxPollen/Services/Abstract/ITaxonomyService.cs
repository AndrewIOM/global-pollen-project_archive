using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface ITaxonomyService
    {
        IEnumerable<Taxon> GetAll();
        IEnumerable<Taxon> GetAllFamilies();
        IEnumerable<Taxon> GetAllGenus();
        IEnumerable<Taxon> GetAllSpecies();
    }
}
