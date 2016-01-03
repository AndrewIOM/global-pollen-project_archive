using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface ITaxonomyService
    {
        void CreateOrUpdateTaxonomy(string family, string genus, string species);
        IEnumerable<Taxon> GetAll();
        IEnumerable<Taxon> GetAll(Taxonomy rank);
        Taxon GetById(int id);
        IEnumerable<ReferenceGrain> GetReferenceGrains(Taxon taxon);
        IEnumerable<Grain> GetUserGrains(Taxon taxon);
    }
}
