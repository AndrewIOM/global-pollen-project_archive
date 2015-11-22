using Microsoft.Data.Entity;
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

        public IEnumerable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public AppUser GetById(string id)
        {
            var result = _context.Users.FirstOrDefault(m => string.Equals(id, m.Id));
            return result;
        }

        public IEnumerable<Organisation> GetOrganisations()
        {
            return _context.Organisations.Include(m => m.Members).ToList();
        }
    }
}
