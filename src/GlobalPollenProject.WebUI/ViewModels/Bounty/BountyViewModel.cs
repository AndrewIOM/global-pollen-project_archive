using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.ViewModels
{
    public class BountyViewModel
    {
        public string Name { get; set; }
        public double Bounty { get; set; }
    }

    public class BountyChartsViewModel
    {
        public List<BountyViewModel> TopOrgs { get; set; }
        public List<BountyViewModel> TopIndividuals { get; set; }
    }
}
