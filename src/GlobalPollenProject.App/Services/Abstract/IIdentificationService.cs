using System.Collections.Generic;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Interfaces
{
    public interface IIdentificationService : IAppService
    {
        /// Identify an unknown grain as the currently logged in user.
        AppServiceResult IdentifyAs (int grainId, string family, string genus, string species);

        /// Current user wants to remove their identification from a user-uploaded grain
        AppServiceResult RemoveIdentification(int grainId);

        /// Upload an unknown grain
        AppServiceResult UploadUnknownGrain(AddUnknownGrain grain);

        /// As a user, I want to be able to delete grains I previously uploaded.
        AppServiceResult RemoveUnknownGrain(int grainId);

        /// View an existing unknown grain
        AppServiceResult<UnknownGrain> GetUnknownGrain(int grainId);

        /// Identify an unknown grain as the currently logged in user.
        AppServiceResult<List<UnknownGrain>> GetMyUnknownGrains(int pageSize, int page);

        /// List out unidentified grains
        AppServiceResult<List<UnknownGrain>> GetUnknownGrains(GrainSearchFilter criteria, int pageSize, int page);
    }
}