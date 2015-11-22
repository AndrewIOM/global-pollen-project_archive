using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
{
    public class TaxonViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string KeyImageUrl { get; set; }
        public int ConfirmedGrainsCount { get; set; }
        public Taxonomy Rank { get; set; }
    }
}
