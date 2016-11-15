using System;

namespace GlobalPollenProject.App.Models
{
    public class IdentificationDto
    {
        public int Id { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime Time { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public Rank Rank { get; set; }
    }
}