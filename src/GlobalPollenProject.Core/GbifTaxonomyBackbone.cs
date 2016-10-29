using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Data.Models.Enums;
using GlobalPollenProject.Core.Models;
using GlobalPollenProject.Core.Utilities;

namespace Im.Acm.Pollen.Services.Concrete
{
    public class GbifTaxonomyBackbone : ITaxonomyBackbone
    {
        private ILogger<GbifTaxonomyBackbone> _logger;
        public GbifTaxonomyBackbone(ILogger<GbifTaxonomyBackbone> logger)
        {
            _logger = logger;
        }

        public bool IsValidTaxon(Taxonomy rank, string family, string genus, string species)
        {
            var result = GbifUtility.GetGbifId(rank, family, genus, species).Result;
            if (result == 0)
            {
                _logger.LogDebug(string.Format("GBIF Taxonomic Lookup failed for {0}, F: {1} G: {2} S: {3}", rank, family, genus, species));
                return false;
            }
            _logger.LogDebug(string.Format("GBIF Taxonomic Lookup succeeded for {0}, F: {1} G: {2} S: {3}", rank, family, genus, species));
            return true;
        }

        public List<BackboneTaxon> Suggest(string name, Taxonomy? rank, string parent = null)
        {
            throw new NotImplementedException();
        }
    }
}
