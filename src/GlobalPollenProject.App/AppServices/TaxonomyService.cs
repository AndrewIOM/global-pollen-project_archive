using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App
{
    public class TaxonomyService : ITaxonomyService
    {
        public void AssessValidity(string family, string genus, string species)
        {
            throw new NotImplementedException();
        }

        public List<BackboneTaxon> Search(string searchTerm, Rank? rank, string parent = null)
        {
            throw new NotImplementedException();
        }
    }
}