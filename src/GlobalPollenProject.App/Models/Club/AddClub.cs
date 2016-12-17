using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.App.Models
{
    public class AddClub
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }
        public string Country { get; set; }
    }
}