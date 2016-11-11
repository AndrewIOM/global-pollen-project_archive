using System;

namespace GlobalPollenProject.App.Models
{
    public class UnknownGrain {
        public int Id { get; set; }
        public double Score { get; set; }
        public string ThumbnailUri { get; set; }
        public DateTime TimeAdded { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public IdentificationStatus Status { get; set; }
    }
}