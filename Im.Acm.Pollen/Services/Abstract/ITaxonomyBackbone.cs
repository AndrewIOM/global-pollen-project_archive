using Im.Acm.Pollen.Models;

namespace Im.Acm.Pollen.Services.Abstract
{
    public interface ITaxonomyBackbone
    {
        bool IsValidTaxon(Taxonomy rank, string family, string genus, string species);
    }
}
