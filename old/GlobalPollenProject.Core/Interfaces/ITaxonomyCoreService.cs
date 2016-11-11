
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITaxonomyCoreService: ICoreService
    {
        Taxon AddOrUpdateTaxon(string family, string genus = null, string species = null);
    }
}
