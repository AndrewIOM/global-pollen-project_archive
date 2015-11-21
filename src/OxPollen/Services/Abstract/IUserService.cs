using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser GetById(string id);
        IEnumerable<string> GetOrganisations();

    }
}
