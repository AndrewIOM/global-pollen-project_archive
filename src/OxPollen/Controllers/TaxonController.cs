using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using Microsoft.Data.Entity;

namespace OxPollen.Controllers
{
    public class TaxonController : Controller
    {
        private readonly Models.OxPollenDbContext _context;
        public TaxonController(Models.OxPollenDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var taxa = _context.Taxa.Include(m => m.Records).ToList();
            var model = taxa.Select(m => new TaxonViewModel()
            {
                CommonName = m.CommonName,
                Id = m.TaxonId,
                LatinName = m.LatinName,
                ImageUrl = m.Records.FirstOrDefault() == null ? "/images/pollensample.jpg" : m.Records.FirstOrDefault().PhotoUrl,
                ConfirmedGrainsCount = m.Records.Count,
                ContentionRating = 1.3 //m.Records.Select(r => r.Identifications.Select(i => i.TaxonName).Distinct().Count()).Sum()
            }).ToList();
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return new BadRequestResult();
            var taxa = _context.Taxa.Include(m => m.Records).ToList();

            var taxon = taxa.FirstOrDefault(m => m.TaxonId == id);
            if (taxon != null)
            {
                return View("View", taxon);
            }
            return new RedirectToActionResult("Index", "ReferenceCollection", null);
        }
    }
}
