using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class GrainImage
    {
        [Key]
        public int GrainImageId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileNameThumbnail { get; set; }
        //[Required]
        //public bool FocusGrain { get; set; }
        //public string LowFocusFileName { get; set; }
        //public string LowMidFocusFileName { get; set; }
        //public string HighMidFocusFileName { get; set; }
        //public string HighFocusFileName { get; set; }

        //Navigation
        public virtual Grain Grain { get; set; }
    }
}
