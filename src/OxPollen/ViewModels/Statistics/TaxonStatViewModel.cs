using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Statistics
{
    public class TaxonStatViewModel
    {
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
        public bool LinkedToNeotoma { get; set; }
    }
}
