
using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App.Interfaces
{
    public interface IIdentificationService : IAppService
    {
        /// Identify an unknown grain as the currently logged in user.
        void IdentifyAs (int grainId, string family, string genus, string species);

        /// Current user wants to remove their identification from a user-uploaded grain
        void RemoveIdentification(int grainId);

        /// Upload an unknown grain
        void UploadUnknownGrain(AddUnknownGrain grain);

        /// As a user, I want to be able to delete grains I previously uploaded.
        void RemoveUnknownGrain(int grainId);

        /// View an existing unknown grain
        UnknownGrain GetUnknownGrain(int grainId);

        /// Identify an unknown grain as the currently logged in user.
        List<UnknownGrain> GetMyUnknownGrains();

        /// List out unidentified grains
        List<UnknownGrain> GetUnknownGrains(GrainSearchFilter criteria);
    }
}