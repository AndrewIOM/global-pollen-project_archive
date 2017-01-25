using GlobalPollenProject.WebUI.Services.Abstract;
using System.Linq;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Data.Concrete;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.WebUI.Services.Concrete
{
    public class LocalPlantListTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly PollenDbContext _context;
        public LocalPlantListTaxonomyBackbone(PollenDbContext context)
        {
            _context = context;
        }

        public bool IsValidTaxon(Taxonomy rank, string family, string genus, string species)
        {
            if (rank == Taxonomy.Family)
            {
                var familyMatch = _context.PlantListTaxa
                    .Where(m => m.Rank == Taxonomy.Family)
                    .Where(m => m.LatinName == family).ToList();
                return familyMatch.Count == 1;
            }

            else if (rank == Taxonomy.Genus)
            {
                var genusMatch = _context.PlantListTaxa.Where(m => m.Rank == Taxonomy.Genus)
                    .Include(m => m.ParentTaxa)
                    .Where(m => m.Status == TaxonomicStatus.Accepted)
                    .Where(m => m.LatinName == genus)
                    .Where(m => m.ParentTaxa.LatinName == family).ToList();
                return genusMatch.Count == 1;
            }

            else if (rank == Taxonomy.Species)
            {
                //Have to split out this section due to bug in EF7 RC1 release
                var familyMatch = _context.PlantListTaxa.Where(m => m.Rank == Taxonomy.Genus)
                    .Include(m => m.ParentTaxa)
                    .Where(m => m.Status == TaxonomicStatus.Accepted)
                    .Where(m => m.LatinName == genus)
                    .Where(m => m.ParentTaxa.LatinName == family).ToList();
                if (familyMatch.Count == 0) return false; //If genus - family link is not valid, return false

                var match = _context.PlantListTaxa.Include(m => m.ParentTaxa)
                    .Where(m => m.Rank == Taxonomy.Species)
                    .Where(m => m.Status == TaxonomicStatus.Accepted)
                    .Where(m => m.LatinName == genus + " " + species)
                    .Where(m => m.ParentTaxa.LatinName == genus).ToList();
                return match.Count == 1;
            }

            //Catch other taxonomic ranks
            return false;
        }
    }
}
