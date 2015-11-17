using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
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
