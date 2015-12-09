﻿using OxPollen.Data.Abstract;
using OxPollen.Models;
using System;

namespace OxPollen.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private OxPollenDbContext _context = null;
        public UnitOfWork(OxPollenDbContext context)
        {
            _context = context;
        }

        private IUserRepository userRepo = null;
        private IRepository<Grain> grainRepo = null;
        private IRepository<Organisation> orgRepo = null;
        private IRepository<Taxon> taxonRepo = null;
        private IRepository<GrainImage> imageRepo = null;
        private IRepository<Identification> idRepo = null;

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepo == null)
                {
                    userRepo = new UserRepository(_context);
                }
                return userRepo;
            }
        }
        public IRepository<Grain> GrainRepository
        {
            get
            {
                if (grainRepo == null)
                {
                    grainRepo = new GrainRepository(_context);
                }
                return grainRepo;
            }
        }
        public IRepository<Organisation> OrganisationRepository
        {
            get
            {
                if (orgRepo == null)
                {
                    orgRepo = new OrganisationRepository(_context);
                }
                return orgRepo;
            }
        }
        public IRepository<Taxon> TaxonRepository
        {
            get
            {
                if (taxonRepo == null)
                {
                    taxonRepo = new TaxonRepository(_context);
                }
                return taxonRepo;
            }
        }
        public IRepository<GrainImage> ImageRepository
        {
            get
            {
                if (imageRepo == null)
                {
                    imageRepo = new ImageRepository(_context);
                }
                return imageRepo;
            }
        }
        public IRepository<Identification> IdentificationRepository
        {
            get
            {
                if (idRepo == null)
                {
                    idRepo = new IdentificationRepository(_context);
                }
                return idRepo;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
