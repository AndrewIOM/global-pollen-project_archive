using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels.Taxon
{
    public class TaxonIndexViewModel
    {
        public List<TaxonViewModel> Taxa { get; set; }

        //Filtering
        public string Query { get; set; }
        public Taxonomy Rank { get; set; }

        //Paging
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
        public int PageSize { get; set; }

        public TaxonIndexViewModel()
        {
            Taxa = new List<TaxonViewModel>();
        }
    }
}
