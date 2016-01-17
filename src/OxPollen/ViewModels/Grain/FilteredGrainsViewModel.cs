using OxPollen.Models;
using System.Collections.Generic;

namespace OxPollen.ViewModels.Grain
{
    public class FilteredGrainsViewModel
    {
        public List<SimpleGrainViewModel> Grains { get; set; }
        public GrainSearchFilter Filters { get; set; }
    }
}
