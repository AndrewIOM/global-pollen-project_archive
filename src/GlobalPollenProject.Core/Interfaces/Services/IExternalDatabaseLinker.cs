using System.Threading.Tasks;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IExternalDatabaseLinker
    {
        Task<int> GetNeotomaDatabaseId(string family, string genus, string species);
        Task<int> GetGlobalBiodiversityInformationFacilityId(string family, string genus, string species);
    }
}