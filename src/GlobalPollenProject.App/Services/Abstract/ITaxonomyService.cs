using System.Collections.Generic;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Interfaces
{
    public interface ITaxonomyService : IAppService
    {
        AppServiceResult AssessValidity (string family, string genus, string species);
        AppServiceResult<List<BackboneTaxon>> SearchBackbone (string searchTerm, int pageSize, int page, Rank? rank, string parent = null);
        
        AppServiceResult<List<PollenProjectTaxon>> ListGPPTaxa(int pageSize, int page);
        AppServiceResult<PollenProjectTaxon> GetTaxon(int id);
        AppServiceResult<List<PollenProjectTaxon>> GetTaxa(string latinName, int pageSize, int page, Rank? rank, string parent = null);
    }
}