using Microsoft.AspNetCore.Mvc;
using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.ViewModels.Statistics;
using System.Linq;
using Im.Acm.Pollen.Data.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Im.Acm.Pollen.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IGrainService _grainService;
        private readonly ITaxonomyService _taxonService;
        private readonly IReferenceService _referenceService;
        private readonly PollenDbContext _context;
        public StatisticsController(IGrainService grainService, 
            ITaxonomyService taxonService,
            IReferenceService referenceService,
            PollenDbContext context)
        {
            _taxonService = taxonService;
            _grainService = grainService;
            _referenceService = referenceService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Taxa()
        {
            var taxa = _taxonService.GetAll().ToList();
            var model = taxa.Select(m => new TaxonStatViewModel()
            {
                Name = m.LatinName,
                Rank = m.Rank,
                LinkedToNeotoma = m.NeotomaId != 0
            });
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Collections()
        {
            var collections = _referenceService.ListCollections();
            var model = collections.Select(m => new CollectionStatViewModel()
            {
                RegionOfInterest = m.FocusRegion,
                SlideCount = m.Grains.Count
            });
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Locations()
        {
            var grains = _grainService.GetUnidentifiedGrains(Models.Taxonomy.Genus).ToList();
            var model = grains.Select(m => new LocationStatViewModel()
            {
                DateAdded = m.TimeAdded,
                Id = m.Id,
                Latitude = m.Latitude,
                Longitude = m.Longitude
            });
            return Ok(model);
        }

        //TODO Remove use of context
        [HttpGet]
        public IActionResult GrainSize(int taxonId) {
            var taxon = _context.Taxa
                .Include(t => t.ReferenceGrains)
                .Include(t => t.UserGrains)
                .Include(t => t.ChildTaxa).ThenInclude(c => c.ReferenceGrains)
                .Include(t => t.ChildTaxa).ThenInclude(m => m.UserGrains)
                .Include(t => t.ChildTaxa).ThenInclude(c => c.ChildTaxa).ThenInclude(cc => cc.ReferenceGrains)
                .Include(t => t.ChildTaxa).ThenInclude(c => c.ChildTaxa).ThenInclude(cc => cc.UserGrains)
            .FirstOrDefault(m => m.TaxonId == taxonId);

            if (taxon == null) return BadRequest();
            
            //Current Taxa Sizes
            var sizes = taxon.ReferenceGrains.Select(m => m.MaxSizeNanoMetres).ToList();
            sizes.AddRange(taxon.UserGrains.Select(m => m.MaxSizeNanoMetres));

            //First Child Sizes
            if (taxon.ChildTaxa.Count > 0) {
                sizes.AddRange(taxon.ChildTaxa.SelectMany(m => m.ReferenceGrains.Select(n => n.MaxSizeNanoMetres)));
                sizes.AddRange(taxon.ChildTaxa.SelectMany(m => m.UserGrains.Select(n => n.MaxSizeNanoMetres)));

                var secondChildren = taxon.ChildTaxa.SelectMany(m => m.ChildTaxa);
                //Second child sizes
                foreach (var secondChild in secondChildren) {
                    sizes.AddRange(secondChild.ReferenceGrains.Select(m => m.MaxSizeNanoMetres));
                    sizes.AddRange(secondChild.UserGrains.Select(m => m.MaxSizeNanoMetres));
                }
            }

            return Ok(sizes);
        }
    }
}
