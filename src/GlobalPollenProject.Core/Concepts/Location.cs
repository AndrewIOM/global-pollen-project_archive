using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalPollenProject.Core.Concepts
{
    // Location in Decimal Degrees
    [ComplexType]
    public class Location
    {
        public Location(double latitude, double longitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
    }
}