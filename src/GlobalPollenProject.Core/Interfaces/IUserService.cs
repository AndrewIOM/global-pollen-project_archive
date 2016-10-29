using System.Collections.Generic;
using GlobalPollenProject.Data.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        IEnumerable<Organisation> GetOrganisations();
        void Update(AppUser user);
    }
}
