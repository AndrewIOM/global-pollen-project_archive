using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Services
{
    public class LocalKewTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly IRepository<KewBackboneTaxon> _taxonRepo;

        public LocalKewTaxonomyBackbone(IRepository<KewBackboneTaxon> taxonRepo)
        {
            _taxonRepo = taxonRepo;
        }

        public bool IsValidTaxon(Rank rank, string family, string genus, string species)
        {
            throw new NotImplementedException();
        }
    }
}