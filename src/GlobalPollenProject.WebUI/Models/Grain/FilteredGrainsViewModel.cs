using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Models.Grain
{
    public class FilteredGrainsViewModel
    {
        public List<SimpleGrainViewModel> Grains { get; set; }
        public GrainSearchFilter Filters { get; set; }
    }
}
