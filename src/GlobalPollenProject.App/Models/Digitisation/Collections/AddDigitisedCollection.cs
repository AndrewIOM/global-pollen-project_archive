using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.App.Models
{
    public class AddDigitisedCollection
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Institution { get; set; }

        [Required]
        public string OwnedBy { get; set; }

        [Required]
        [Description("The country where the collection is located")]
        [Display(Name = "Country")]
        public string CountryCode { get; set; }

        [Description("The global region for which the majority of taxa were collected. If the collection spans multiple regions, leave this blank.")]
        [Display(Name = "Regional Focus")]
        public string FocusRegion { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Website Address")]
        [DataType(DataType.Url)]
        public string WebAddress { get; set; }

        [Display(Name = "Email Address for Enquiries")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
    }
}
