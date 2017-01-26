using System.Collections.Generic;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITaxonomyBackbone
    {
        BackboneTaxonAggregate Match(string family, string genus, string species);
        bool IsValidTaxon(Rank rank, string family, string genus, string species);
        List<BackboneTaxonAggregate> Suggest(string latinName, Rank? rank, string parent = null);
    }
}
