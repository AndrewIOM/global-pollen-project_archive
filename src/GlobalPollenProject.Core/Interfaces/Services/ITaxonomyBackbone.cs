using System.Collections.Generic;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITaxonomyBackbone
    {
        KewBackboneTaxon Match(string family, string genus, string species);
        bool IsValidTaxon(Rank rank, string family, string genus, string species);
        List<KewBackboneTaxon> Suggest(string latinName, Rank? rank, string parent = null);
    }
}
