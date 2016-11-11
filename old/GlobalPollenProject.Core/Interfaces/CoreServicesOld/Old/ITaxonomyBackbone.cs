using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITaxonomyBackbone
    {
        bool IsValidTaxon(Taxonomy rank, string family, string genus, string species);
        // List<BackboneTaxon> Suggest(string name, Taxonomy? rank, string parent = null);
    }
}
