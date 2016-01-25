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
        public bool IsValidTaxon(Taxonomy rank, string family, string genus, string species)
        {
            var result = GbifUtility.GetGbifId(rank, family, genus, species).Result;
            if (result == 0) return false;
            return true;
        }
    }
}
