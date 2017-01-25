using GlobalPollenProject.WebUI.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.ViewModels.Grain
{
    public class FilteredGrainsViewModel
    {
        public List<SimpleGrainViewModel> Grains { get; set; }
        public GrainSearchFilter Filters { get; set; }
    }
}
