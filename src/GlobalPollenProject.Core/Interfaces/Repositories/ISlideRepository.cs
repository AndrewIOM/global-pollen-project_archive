using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;

public interface ISlideRepository : IRepository<ReferenceSlide>
{
    PagedResult<ReferenceSlide> GetSlidesForTaxon(int taxonId, bool recursive, int page, int pageSize);
}
