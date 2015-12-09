﻿using OxPollen.Data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class Grain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual List<GrainImage> Images { get; set; }
        [Required]
        public virtual AppUser SubmittedBy { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public DateTime TimeAdded { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        //Optional
        public int? AgeYearsBeforePresent { get; set; }

        //Identifications
        public virtual List<Identification> Identifications { get; set; }
        public string Species { get; set; }
        public string Genus { get; set; }
        public string Family { get; set; }

        public Grain()
        {
            IsDeleted = false;
        }
    }
}
