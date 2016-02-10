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

        //Navigation
        public virtual Grain Grain { get; set; }
    }
}
