using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.WebUI.Models.Reference
{
    public class RequestAccessViewModel
    {
        [Required]
        public string Comments { get; set; }

        public bool HasRequestedAccess { get; set; }
    }
}
