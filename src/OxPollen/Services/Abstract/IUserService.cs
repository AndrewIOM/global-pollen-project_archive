using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        IEnumerable<Organisation> GetOrganisations();

    }
}
