using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.WebUI.ViewModels.Reference
{
    public class RequestAccessViewModel
    {
        [Required]
        public string Comments { get; set; }

        public bool HasRequestedAccess { get; set; }
    }
}
