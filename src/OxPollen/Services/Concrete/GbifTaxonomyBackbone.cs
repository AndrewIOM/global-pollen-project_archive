using Microsoft.Extensions.Logging;
using OxPollen.Models;
using OxPollen.Services.Abstract;
using OxPollen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Concrete
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
    }
}
