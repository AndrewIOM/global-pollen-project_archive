using System.ComponentModel.DataAnnotations;

namespace Im.Acm.Pollen.ViewModels.Reference
{
    public class RequestAccessViewModel
    {
        [Required]
        public string Comments { get; set; }

        public bool HasRequestedAccess { get; set; }
    }
}
