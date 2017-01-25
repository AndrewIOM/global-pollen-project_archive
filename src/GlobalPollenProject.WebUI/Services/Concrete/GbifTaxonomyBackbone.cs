using Microsoft.Extensions.Logging;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Services.Abstract;
using GlobalPollenProject.WebUI.Utilities;

namespace GlobalPollenProject.WebUI.Services.Concrete
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
