using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class GrainImage
    {
        [Key]
        public int GrainImageId { get; set; }

        public string FileName { get; set; }
        public double ScaleNanoMetres { get; set; }

        //Navigation
        public Grain Grain { get; set; }
    }
}
