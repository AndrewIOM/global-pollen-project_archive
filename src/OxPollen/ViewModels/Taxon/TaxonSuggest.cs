using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Taxon
{
    public class TaxonSuggest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
    }
}
