using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class IdentificationStatus
    {
        public string ConfirmedSpecies { get; set; }
        public string ConfirmedGenus { get; set; }
        public string ConfirmedFamily { get; set; }
        public List<IdentificationDto> Identifications { get; set; }
    }
}