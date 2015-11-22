using System;

namespace OxPollen.Models
{
    public class Identification
    {
        public int IdentificationId { get; set; }
        public DateTime Time { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public Taxonomy Rank { get; set; }

        //Navigation Properties
        public virtual Grain Grain { get; set; }
        public virtual AppUser User { get; set; }
    }
}
