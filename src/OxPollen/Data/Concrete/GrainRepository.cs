﻿using OxPollen.Data.Abstract;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;

namespace OxPollen.Data.Concrete
{
    public class GrainRepository : IGrainRepository
    {
        private OxPollenDbContext _context;
        public GrainRepository(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(Grain entity)
        {
            _context.UserGrains.Add(entity);
        }

        public void Update(Grain entity)
        {
            _context.UserGrains.Update(entity);
        }

        public void Delete(Grain entity)
        {
            entity.IsDeleted = true;
            _context.UserGrains.Update(entity);
        }

        public IEnumerable<Grain> Find(Expression<Func<Grain, bool>> where)
        {
            return _context.UserGrains.Include(m => m.Identifications)
                .Include(m => m.Images).Where(m => !m.IsDeleted).Where(where);
        }

        public IEnumerable<Grain> GetAll()
        {
            return _context.UserGrains.Where(m => !m.IsDeleted).Include(m => m.Identifications)
                .Include(m => m.Images);
        }

        public IEnumerable<Grain> GetAllDeleted()
        {
            return _context.UserGrains.Where(m => m.IsDeleted).Include(m => m.Identifications)
                .Include(m => m.Images);
        }

        public Grain GetById(int id)
        {
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .Include(m => m.Identifications)
                .Include(m => m.Images).ToList()
                .FirstOrDefault(m => m.Id == id);
            return result;
        }
    }
}