namespace GlobalPollenProject.Core.Interfaces
{
    public interface IExternalDatabaseLinker
    {
        int GetNeotomaDatabaseId(string family, string genus, string species);
        int GetGlobalBiodiversityInformationFacilityId(string family, string genus, string species);
    }
}