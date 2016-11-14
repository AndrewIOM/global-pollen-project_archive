using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Services
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly IUnitOfWork _uow;
        public TaxonomyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

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
            var result = new AppServiceResult<List<PollenProjectTaxon>>();

            var domainResult = _uow.TaxonRepository.GetAll(page, pageSize).Results;
            var dtoResult = domainResult.Select(m => new PollenProjectTaxon()
            {
                Id = m.Id,
                LatinName = m.LatinName,
                Rank = (App.Models.Rank) m.Rank,
                ImageFilename = "",
                UserSubmissionsConfirmedCount = 1,
                ReferenceGrainsCount = 1,
                GbifId = m.GbifId,
                NeotomaId = m.NeotomaId
            }).ToList();

            result.AddResult(dtoResult);
            return result;
        }

        public AppServiceResult<List<BackboneTaxon>> SearchBackbone(string searchTerm, int pageSize, int page, Rank? rank, string parent = null)
        {
            throw new NotImplementedException();
        }
    }
}