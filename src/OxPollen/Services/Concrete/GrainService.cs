﻿using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxPollen.Models;
using Microsoft.Data.Entity;

namespace OxPollen.Services.Concrete
{
    public class GrainService : IGrainService
    {
        private readonly OxPollenDbContext _context;
        public GrainService(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(Grain newRecord)
        {
            _context.Add(newRecord);
            _context.SaveChanges();
        }

        public Grain GetById(int id)
        {
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .Include(m => m.Identifications)
                .Include(m => m.Images).ToList()
                .FirstOrDefault(m => m.GrainId == id);
            return result;
        }

        public IEnumerable<Grain> GetByUser(string userId)
        {
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .Include(m => m.Identifications).Include(m => m.SubmittedBy)
                .Include(m => m.Images)
                .Where(m => m.SubmittedBy.Id == userId);
            return result;
        }

        public IEnumerable<Grain> GetUnidentifiedGrains() //NB Move to ID Service instead?
        {
            var idService = new IdentificationService(_context); //TODO DI this reference
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .Include(m => m.Images)
                .Include(m => m.Identifications)
                .Where(m => !idService.HasConfirmedIdentity(m));
            return result;
        }

        public Grain MarkDeleted(Grain grain)
        {
            var result = _context.UserGrains
                .Where(m => !m.IsDeleted)
                .FirstOrDefault(m => m.GrainId == grain.GrainId);
            if (result == null) return null;
            result.IsDeleted = true;
            _context.Update(result);
            _context.SaveChanges();
            return result;
        }

        public Grain Update(Grain grain)
        {
            throw new NotImplementedException();
        }
    }
}
