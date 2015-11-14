using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class PollenRecord
    {
        public int PollenRecordId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please assign a latitude to your record")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Please assign a longitude to your record")]
        public double Longitude { get; set; }
        public double? ApproximateAge { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime TimeAdded { get; set; }

        //Identification
        public virtual List<Identification> Identifications { get; set; }
        public bool HasConfirmedIdentity { get; set; }
        public DateTime TimeIdentityConfirmed { get; set; }
        public Taxon Taxon { get; set; }

        //Bounty
        public int Bounty
        {
            get
            {
                int daysSinceSubmission = (DateTime.Now - TimeAdded).Days;
                return daysSinceSubmission;
            }
        }

        public PollenRecord()
        {
            HasConfirmedIdentity = false;
        }
    }
}
