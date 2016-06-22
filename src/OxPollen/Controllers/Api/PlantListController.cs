﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using OxPollen.Data.Concrete;
using OxPollen.Models;
using OxPollen.Utilities;
using OxPollen.ViewModels.Api;

namespace OxPollen.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlantListController : Controller
    {
        private readonly OxPollenDbContext _context;
        public PlantListController(OxPollenDbContext context)
        {
            _context = context;
        }

        [HttpGet("suggest")]
        public IEnumerable<BackboneTaxon> Suggest(string q, Taxonomy? rank, string parent = null)
        {
            var result = _context.PlantListTaxa.Include(m => m.ParentTaxa)
                .Where(m => m.LatinName.StartsWith(q, StringComparison.InvariantCultureIgnoreCase)); //TODO Fuzzy matching
            if (rank.HasValue)
            {
                result = result.Where(m => m.Rank == rank);
            }
            if (!string.IsNullOrEmpty(parent))
            {
                result = result.Where(m => m.ParentTaxa.LatinName.Equals(parent, StringComparison.InvariantCultureIgnoreCase));
            }

            var list = result.Take(10).ToList();
            var model = list.Select(m => new BackboneTaxon()
            {
                Id = m.Id,
                LatinName = m.LatinName,
                Rank = m.Rank,
                Status = m.Status,
                ParentLatinName = m.Rank == Taxonomy.Family ? "" : m.ParentTaxa.LatinName
            });

            return model;
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public BackboneTaxon Get(int id)
        //{
        //    return _context.PlantListTaxa.FirstOrDefault(m => m.Id == id);
        //}
    }
}