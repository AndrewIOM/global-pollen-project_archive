using OxPollen.Models;
using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxPollen.Services.Concrete
{
    public class UserService : IUserService
    {
        private OxPollenDbContext _context;

        public UserService(OxPollenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUser GetById(string id)
        {
            var result = _context.Users.FirstOrDefault(m => string.Equals(id, m.Id));
            return result;
        }

        public IEnumerable<string> GetOrganisations()
        {
            var orgs = _context.Users.Select(m => m.Organisation).Distinct();
            return orgs;
        }
    }
}
