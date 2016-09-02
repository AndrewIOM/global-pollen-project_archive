using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.ViewModels.Taxon
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
