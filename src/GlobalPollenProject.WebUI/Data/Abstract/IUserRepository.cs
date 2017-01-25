using GlobalPollenProject.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GlobalPollenProject.WebUI.Data.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        IEnumerable<AppUser> Find(Expression<Func<AppUser, bool>> where);

        void Add(AppUser entity);
        void Update(AppUser entity);
        void Delete(AppUser entity);
    }
}
