using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models.Grain
{
    public class FilteredGrainsViewModel
    {
        public List<UnknownGrain> Grains { get; set; }
        public GrainSearchFilter Filters { get; set; }
    }
}
