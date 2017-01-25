using GlobalPollenProject.WebUI.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Services.Abstract
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        IEnumerable<Organisation> GetOrganisations();
        void Update(AppUser user);
    }
}
