﻿using OxPollen.Data.Abstract;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace OxPollen.Data.Concrete
{
    public class UserRepository : IUserRepository
    {
        private OxPollenDbContext _context;
        public UserRepository(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(AppUser entity)
        {
            _context.Users.Add(entity);
        }

        public void Update(AppUser entity)
        {
            _context.Update(entity);
        }

        public void Delete(AppUser entity)
        {
            _context.Users.Update(entity);
        }

        public IEnumerable<AppUser> Find(Expression<Func<AppUser, bool>> where)
        {
            return _context.Users.Include(m => m.Organisation).Where(where);
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public AppUser GetById(string id)
        {
            return _context.Users.FirstOrDefault(m => m.Id == id);
        }
    }
}
