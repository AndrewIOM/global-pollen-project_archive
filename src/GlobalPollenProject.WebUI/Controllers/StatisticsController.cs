using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class StatisticsController : Controller
    {
        // private readonly IGrainService _grainService;
        // private readonly ITaxonomyService _taxonService;
        // private readonly IReferenceService _referenceService;
        // public StatisticsController(IGrainService grainService, 
        //     ITaxonomyService taxonService,
        //     IReferenceService referenceService)
        // {
        //     _taxonService = taxonService;
        //     _grainService = grainService;
        //     _referenceService = referenceService;
        // }

        // [HttpGet]
        // public IActionResult Taxa()
        // {
        //     var taxa = _taxonService.GetAll().ToList();
        //     var model = taxa.Select(m => new TaxonStatViewModel()
        //     {
        //         Name = m.LatinName,
        //         Rank = m.Rank,
        //         LinkedToNeotoma = m.NeotomaId != 0
        //     });
        //     return Ok(model);
        // }

        // [HttpGet]
        // public IActionResult Collections()
        // {
        //     var collections = _referenceService.ListCollections();
        //     var model = collections.Select(m => new CollectionStatViewModel()
        //     {
        //         RegionOfInterest = m.FocusRegion,
        //         SlideCount = m.Grains.Count
        //     });
        //     return Ok(model);
        // }

        // [HttpGet]
        // public IActionResult Locations()
        // {
        //     var grains = _grainService.GetUnidentifiedGrains(Models.Taxonomy.Genus).ToList();
        //     var model = grains.Select(m => new LocationStatViewModel()
        //     {
        //         DateAdded = m.TimeAdded,
        //         Id = m.Id,
        //         Latitude = m.Latitude,
        //         Longitude = m.Longitude
        //     });
        //     return Ok(model);
        // }

    }
}
