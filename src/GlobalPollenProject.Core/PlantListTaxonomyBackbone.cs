using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using GlobalPollenProject.Data.Infrastructure;
using GlobalPollenProject.Data.Models.Enums;
using GlobalPollenProject.Core.Models;
using GlobalPollenProject.Data.Models;
using GlobalPollenProject.Core.Interfaces;

namespace Im.Acm.Pollen.Services.Concrete
{
    public class PlantListTaxonomyBackbone : ITaxonomyBackbone
    {
        private readonly PollenDbContext _context;
        public PlantListTaxonomyBackbone(PollenDbContext context)
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

        public List<BackboneTaxon> Suggest(string name, Taxonomy? rank, string parent = null)
        {
            IQueryable<PlantListTaxon> result = _context.PlantListTaxa.Include(m => m.ParentTaxa);
            if (!string.IsNullOrEmpty(parent)) { result = result.Where(m => m.ParentTaxa.LatinName.Equals(parent, StringComparison.OrdinalIgnoreCase)); }
            if (rank.HasValue) { result = result.Where(m => m.Rank == rank); }
            result = result.Where(m => m.LatinName.StartsWith(name));

            var list = result.OrderBy(m => m.LatinName).Take(10).ToList();
            var backboneResult = list.Select(m => new BackboneTaxon()
            {
                Id = m.Id,
                LatinName = m.LatinName,
                Rank = m.Rank,
                Status = m.Status,
                ParentLatinName = m.Rank == Taxonomy.Family ? "" : m.ParentTaxa.LatinName
            }).ToList();
            return backboneResult;
        }
    }
}
