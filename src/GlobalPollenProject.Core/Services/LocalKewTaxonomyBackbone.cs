using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Services
{
    public class LocalKewTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly IRepository<IBackboneTaxon> _taxonRepo;

        public LocalKewTaxonomyBackbone(IRepository<IBackboneTaxon> taxonRepo)
        {
            _taxonRepo = taxonRepo;
        }

        public bool IsValidTaxon(Rank rank, string family, string genus, string species)
        {
            throw new NotImplementedException();
        }
    }
}