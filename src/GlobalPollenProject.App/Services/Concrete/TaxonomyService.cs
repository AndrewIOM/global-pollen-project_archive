using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Mapping;
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

        public AppServiceResult<SizeProfile> GetSizeProfile(int taxonId)
        {
            var profile = new SizeProfile();
            profile.Mean = 22.0;
            profile.Measurements = new double[] {21, 22, 23, 21, 22};
            
            return new AppServiceResult<SizeProfile>(profile);
        }

        public AppServiceResult<BackboneTaxon> GetStatus(string family, string genus, string species)
        {
            var result = new AppServiceResult<BackboneTaxon>();

            var match = _uow.BackboneCoreService.Match(family, genus, species);
            if (match == null)
            {
                result.AddMessage("", "A match could not be found", AppServiceMessageType.Error);
            }
            result.AddResult(match.ToDto());
            return result;
        }

        public PagedAppServiceResult<PollenProjectTaxon> GetTaxa(string latinName, int pageSize, int page, App.Models.Rank? rank, string parent = null)
        {
            var domainResult = _uow.TaxonRepository.FindBy(m => m.LatinName.Contains(latinName), page, pageSize);
            var dtoResult = domainResult.Results.Select(m => m.ToDto()).ToList();
            var result = new PagedAppServiceResult<PollenProjectTaxon>(dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

        public AppServiceResult<PollenProjectTaxon> GetTaxon(int id)
        {
            var result = new AppServiceResult<PollenProjectTaxon>();

            var domainResult = _uow.TaxonRepository.FirstOrDefault(m => m.Id == id);
            if (domainResult == null)
            {
                result.AddMessage("", "The taxon specified does not exist", AppServiceMessageType.Error);
                return result;
            }

            result.AddResult(domainResult.ToDto());
            return result;
        }

        public PagedAppServiceResult<PollenProjectTaxon> ListGPPTaxa(TaxonFilter filter, int pageSize, int page)
        {
            var result = new PagedAppServiceResult<PollenProjectTaxon>();
            var domainResult = _uow.TaxonRepository.FindBy(m => 
                    (filter.Rank.HasValue ? m.Rank == (Core.Rank)filter.Rank.Value : true)
                    //&& (!string.IsNullOrEmpty(filter.LatinName) ? true : m.LatinName.Contains(filter.LatinName))
                    , page, pageSize);

            var dtoResult = domainResult.Results.Select(m => m.ToDto()).ToList();
            result.AddResult(dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

        public AppServiceResult<List<BackboneTaxon>> SearchBackbone(string searchTerm, App.Models.Rank? rank, string parent = null)
        {
            var domainResult = _uow.BackboneCoreService.Suggest(searchTerm, (Core.Rank)rank, parent);
            var dtoResult = domainResult.Select(m => m.ToDto()).ToList();

            var result = new AppServiceResult<List<BackboneTaxon>>(dtoResult);
            return result;
        }
    }
}