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
            KewBackboneTaxon match;
            if (rank == Rank.Family)
            {
                match = _taxonRepo.FirstOrDefault(m => m.LatinName == family && m.Rank == Rank.Family);
            }
            else if (rank == Rank.Genus)
            {
                match = _taxonRepo.FirstOrDefault(m => m.LatinName == genus && m.Rank == Rank.Genus && m.ParentTaxa.LatinName == family);
            } 
            else
            {
                match = _taxonRepo.FirstOrDefault(m => m.LatinName == species && m.Rank == Rank.Species
                    && m.ParentTaxa.LatinName == genus && m.ParentTaxa.ParentTaxa.LatinName == family);
            }

            if (match == null) return false;
            return true;
        }
    }
}