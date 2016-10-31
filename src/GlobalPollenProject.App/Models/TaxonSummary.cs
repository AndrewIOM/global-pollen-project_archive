namespace GlobalPollenProject.App.Models
{
    public class TaxonSummary 
    {
        public int TaxonId { get; set; }
        public TaxonomicStatus Status { get; set; }
        public int DigitisedSlidesCount { get; set; }
        public int UserSubmissionsCount { get; set; }
        public string KeyImageUrl { get; set; }
    }
}