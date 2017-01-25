using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.WebUI.Models
{
    public class GrainImage
    {
        [Key]
        public int GrainImageId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileNameThumbnail { get; set; }

        //Navigation
        public virtual Grain Grain { get; set; }

        //Focus Image
        public bool IsFocusImage { get; set; }
        public string FocusLowUrl { get; set; }
        public string FocusMedLowUrl { get; set; }
        public string FocusMedUrl { get; set; }
        public string FocusMedHighUrl { get; set; }
        public string FocusHighUrl { get; set; }
    }
}