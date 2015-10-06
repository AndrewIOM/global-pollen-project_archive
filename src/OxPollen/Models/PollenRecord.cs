using Microsoft.AspNet.Identity.EntityFramework;

namespace OxPollen.Models
{
    public class PollenRecord
    {
        public int PollenRecordId { get; set; }
        public IdentityUser User { get; set; }
        public string PhotoUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double ApproximateAge { get; set; }

        public bool HasConfirmedIdentity { get; set; }
        public Taxon Taxon { get; set; }
    }
}
