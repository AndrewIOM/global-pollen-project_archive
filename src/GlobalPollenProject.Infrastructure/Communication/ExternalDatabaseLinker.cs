using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Infrastructure.Communication
{
    public class ExternalDatabaseLinker : IExternalDatabaseLinker
    {
        public int GetGlobalBiodiversityInformationFacilityId(string family, string genus, string species)
        {
            throw new NotImplementedException();
        }

        public int GetNeotomaDatabaseId(string family, string genus, string species)
        {
            throw new NotImplementedException();
        }
    }
}