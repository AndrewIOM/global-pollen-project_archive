using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
{
    public class GrainViewModel
    {
        public int Id { get; set; }
        public double Bounty { get; set; }
        public string ImageLocation { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
