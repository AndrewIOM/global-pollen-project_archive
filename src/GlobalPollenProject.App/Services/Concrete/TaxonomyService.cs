using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Services
{
    public class TaxonomyService : ITaxonomyService
    {
        public AppServiceResult AssessValidity(string family, string genus, string species)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<PollenProjectTaxon>> GetTaxa(string latinName, int pageSize, int page, Rank? rank, string parent = null)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<PollenProjectTaxon> GetTaxon(int id)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<PollenProjectTaxon>> ListGPPTaxa(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<BackboneTaxon>> SearchBackbone(string searchTerm, int pageSize, int page, Rank? rank, string parent = null)
        {
            throw new NotImplementedException();
        }
    }
}