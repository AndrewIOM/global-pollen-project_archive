using OxPollen.Services.Abstract;
using System;
using System.Linq;
using OxPollen.Models;
using OxPollen.Data.Concrete;
using Microsoft.Data.Entity;

namespace OxPollen.Services.Concrete
{
    public class LocalPlantListTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly OxPollenDbContext _context;
        public LocalPlantListTaxonomyBackbone(OxPollenDbContext context)
        {
            _context = context;
        }

        public bool IsValidTaxon(Taxonomy rank, string family, string genus, string species)
        {
            if (rank == Taxonomy.Family)
            {
                var match = _context.PlantListTaxa.FirstOrDefault(m => m.LatinName.Equals(family, StringComparison.InvariantCultureIgnoreCase)
                    && m.Rank == Taxonomy.Family);
                return match != null;
            }

            else if (rank == Taxonomy.Genus)
            {
                var match = _context.PlantListTaxa.Include(m => m.ParentTaxa)
                    .FirstOrDefault(m => m.LatinName.Equals(genus, StringComparison.InvariantCultureIgnoreCase)
                        && m.Rank == Taxonomy.Genus
                        && m.ParentTaxa.LatinName.Equals(family, StringComparison.InvariantCultureIgnoreCase));
                return match != null;
            }

            else if (rank == Taxonomy.Species)
            {
                var match = _context.PlantListTaxa.Include(m => m.ParentTaxa).ThenInclude(n => n.ParentTaxa)
                    .FirstOrDefault(m => m.LatinName.Equals(species, StringComparison.InvariantCultureIgnoreCase)
                        && m.Rank == Taxonomy.Species
                        && m.ParentTaxa.LatinName.Equals(genus, StringComparison.InvariantCultureIgnoreCase)
                        && m.ParentTaxa.ParentTaxa.LatinName.Equals(family, StringComparison.InvariantCultureIgnoreCase));
                return match != null;
            }

            //Catch other taxonomic ranks
            return false;
        }
    }
}
