using GlobalPollenProject.WebUI.Models;

namespace GlobalPollenProject.WebUI.Services.Abstract
{
    public interface ITaxonomyBackbone
    {
        bool IsValidTaxon(Taxonomy rank, string family, string genus, string species);
    }
}
