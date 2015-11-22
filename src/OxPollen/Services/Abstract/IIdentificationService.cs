using OxPollen.Models;
using System.Collections.Generic;

namespace OxPollen.Services.Abstract
{
    public interface IIdentificationService
    {
        bool HasConfirmedIdentity(Grain grain);
        string GetFamily(Grain grain);
        string GetGenus(Grain grain);
        string GetSpecies(Grain grain);
        bool IsIdentifiedByUser(int grainId, string userId);
        void SaveIdentification(Identification newIdentification);
    }
}
