using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models.Taxon
{
    public class TaxonIndexViewModel
    {
        public List<TaxonViewModel> Taxa { get; set; }

        //Filtering
        public string Query { get; set; }
        public Rank Rank { get; set; }

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
