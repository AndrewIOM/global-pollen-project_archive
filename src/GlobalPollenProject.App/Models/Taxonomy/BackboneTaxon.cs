
namespace GlobalPollenProject.App.Models
{
    public class BackboneTaxon
    {
        public int Id { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Rank Rank { get; set; }
        public string LatinName { get; set; }
        public string ParentLatinName { get; set; }

        public string LatinNameAuthorship { get; set; }
        public string Reference { get; set; }
        public string ReferenceUrl { get; set; }
    }
}
