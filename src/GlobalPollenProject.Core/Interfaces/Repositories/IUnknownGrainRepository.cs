using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;

public interface IUnknownGrainRepository : IRepository<UnknownGrain>
{
    PagedResult<ReferenceSlide> GetGrainsIdentifiedAs(int taxonId);
}
