using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class DigitisedCollection {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Institution { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }

        public string CountryCode { get; set; }
        public string FocusRegion { get; set; }
        public List<DigitisedSlide> Slides { get; set; }
    }
}