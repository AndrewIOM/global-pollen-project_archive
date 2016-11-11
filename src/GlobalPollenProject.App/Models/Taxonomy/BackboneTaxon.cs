
namespace GlobalPollenProject.App.Models
{
    public class BackboneTaxon
    {
        public int Id { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Rank Rank { get; set; }
        public string LatinName { get; set; }
        public string ParentLatinName { get; set; }
    }
}
