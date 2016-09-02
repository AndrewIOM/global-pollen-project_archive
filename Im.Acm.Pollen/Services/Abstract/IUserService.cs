using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.Services.Abstract
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        IEnumerable<Organisation> GetOrganisations();
        void Update(AppUser user);
    }
}
