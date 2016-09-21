using System;
using System.Collections.Generic;
using System.Linq;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.Services.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Im.Acm.Pollen.Controllers.Api
{
    [Route("api/[controller]")]
    public class TaxonomyController : Controller
    {
        private readonly ITaxonomyService _service;
        private readonly ITaxonomyBackbone _backbone;

        public TaxonomyController() {

        }



        public IActionResult Match(string latinName, Taxonomy rank) {
            var result = _service.GetAll().FirstOrDefault(m => m.LatinName == latinName && m.Rank == rank);
            var model = new TaxonDTO() {
                TaxonId = result.TaxonId
            };
            return Ok(model);
        }

        private class TaxonDTO 
        {
            public int TaxonId { get; set; }
            public TaxonomicStatus Status { get; set; }
            public int DigitisedSlidesCount { get; set; }
            public int UserSubmissionsCount { get; set; }
            public string KeyImageUrl { get; set; }
        }
    }
}