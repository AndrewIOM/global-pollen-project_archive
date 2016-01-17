using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
{
    public class SimpleGrainViewModel
    {
        public int Id { get; set; }
        public double Bounty { get; set; }
        public string ImageLocation { get; set; }
        public string ThumbnailLocation { get; set; }
        public DateTime TimeAdded { get; set; }

        public string ConfirmedSpecies { get; set; }
        public string ConfirmedGenus { get; set; }
        public string ConfirmedFamily { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
