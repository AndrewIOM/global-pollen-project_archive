using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface ITaxonomyBackbone
    {
        bool IsValidTaxon(Taxonomy rank, string family, string genus, string species);
    }
}
