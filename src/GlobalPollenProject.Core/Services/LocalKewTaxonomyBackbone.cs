using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Services
{
    public class LocalKewTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly IRepository<BackboneTaxonAggregate> _taxonRepo;

        public LocalKewTaxonomyBackbone(IRepository<BackboneTaxonAggregate> taxonRepo)
        {
            _taxonRepo = taxonRepo;
        }

        public bool IsValidTaxon(Rank rank, string family, string genus, string species)
        {
            BackboneTaxonAggregate match;
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

        public BackboneTaxonAggregate Match(string family, string genus, string species)
        {
            var rank = Rank.Family;
            if (string.IsNullOrEmpty(family)) return null;
            if (!string.IsNullOrEmpty(genus)) rank = Rank.Genus;
            if (!string.IsNullOrEmpty(species)) rank = Rank.Species;
            
            if (rank == Rank.Family)
            {
                var match = _taxonRepo.FirstOrDefault(m => m.Rank == Rank.Family && m.LatinName == family);
                return match;
            }

            if (rank == Rank.Genus)
            {
                var match = _taxonRepo.FirstOrDefault(m => m.Rank == Rank.Genus 
                    && m.LatinName == genus && m.ParentTaxa.LatinName == family);
                return match;
            }

            if (rank == Rank.Species)
            {
                var match = _taxonRepo.FirstOrDefault(m => m.Rank == Rank.Species 
                    && m.LatinName == species && m.ParentTaxa.LatinName == genus);
                return match;
            }
            return null;
        }

        public List<BackboneTaxonAggregate> Suggest(string latinName, Rank? rank, string parent = null)
        {
            var match = _taxonRepo.FindBy(m => m.LatinName.Contains(latinName) 
                && (rank.HasValue ? m.Rank == rank.Value : true)
                && (!string.IsNullOrEmpty(parent) ? m.ParentTaxa.LatinName == parent : true), 1, 10);
            return match.Results.ToList();
        }
    }
}