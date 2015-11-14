using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
{
    public class IdentificationViewModel
    {
        //Display Values
        public int GrainId { get; set; }
        public PollenRecord Grain { get; set; }
        public bool AlreadyIdentifiedByUser { get; set; }

        //Identification Values
        [Required]
        [Display(Name = "Taxon Name")]
        public string TaxonName { get; set; }
    }
}