using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.ViewModels.Grain
{
    public class FilteredGrainsViewModel
    {
        public List<SimpleGrainViewModel> Grains { get; set; }
        public GrainSearchFilter Filters { get; set; }
    }
}
