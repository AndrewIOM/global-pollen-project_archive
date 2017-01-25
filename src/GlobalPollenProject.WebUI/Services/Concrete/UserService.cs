using GlobalPollenProject.WebUI.Data.Abstract;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Services.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace GlobalPollenProject.WebUI.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<AppUser> GetAll()
        {
            var result = _uow.UserRepository.GetAll();
            return result;
        }

        public AppUser GetById(string id)
        {
            var result = _uow.UserRepository.Find(m => m.Id == id).FirstOrDefault();
            return result;
        }

        public IEnumerable<Organisation> GetOrganisations()
        {
            var result = _uow.OrganisationRepository.GetAll();
            return result;
        }

        public void Update(AppUser user)
        {
            _uow.UserRepository.Update(user);
            _uow.SaveChanges();
        }
    }
}
