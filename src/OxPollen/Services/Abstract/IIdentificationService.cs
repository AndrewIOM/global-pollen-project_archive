using OxPollen.Models;
using System.Collections.Generic;

namespace OxPollen.Services.Abstract
{
    public interface IIdentificationService
    {
        bool HasConfirmedIdentity(Grain grain);
        void Remove(Identification identification);
        Identification GetById(int id);
        string GetFamily(Grain grain);
        string GetGenus(Grain grain);
        string GetSpecies(Grain grain);
        Identification GetUsersIdentification(int grainId, string userId);
        void SaveIdentification(Identification newIdentification);
    }
}
