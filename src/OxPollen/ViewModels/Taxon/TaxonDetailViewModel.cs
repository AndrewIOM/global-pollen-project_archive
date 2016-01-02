using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Taxon
{
    public class TaxonDetailViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Taxonomy Rank { get; set; }

        public List<Grain> SubmittedGrains { get; set; }
        public List<ReferenceGrain> ReferenceGrains { get; set; }
    }
}
