using System;

namespace GlobalPollenProject.WebUI.ViewModels.Statistics
{
    public class LocationStatViewModel
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
