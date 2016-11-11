namespace GlobalPollenProject.Core.Interfaces
{
    public interface ITaxonomyBackbone
    {
        bool IsValidTaxon(Rank rank, string family, string genus, string species);
    }
}
