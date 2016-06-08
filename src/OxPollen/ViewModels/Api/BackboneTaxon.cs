using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Api
{
    public class BackboneTaxon
    {
        public int Id { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Taxonomy Rank { get; set; }
        public string LatinName { get; set; }
        public string ParentLatinName { get; set; }
    }
}
