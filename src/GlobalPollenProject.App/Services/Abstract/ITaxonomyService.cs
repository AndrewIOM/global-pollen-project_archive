using System.Collections.Generic;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Interfaces
{
    public interface ITaxonomyService : IAppService
    {
        AppServiceResult<BackboneTaxon> GetStatus (string family, string genus, string species);
        AppServiceResult<List<BackboneTaxon>> SearchBackbone (string searchTerm, Rank? rank, string parent = null);
        
        PagedAppServiceResult<PollenProjectTaxon> ListGPPTaxa(TaxonFilter filter, int pageSize, int page);
        AppServiceResult<PollenProjectTaxon> GetTaxon(int id);
        PagedAppServiceResult<PollenProjectTaxon> GetTaxa(string latinName, int pageSize, int page, Rank? rank, string parent = null);

        AppServiceResult<SizeProfile> GetSizeProfile(int taxonId);
    }
}