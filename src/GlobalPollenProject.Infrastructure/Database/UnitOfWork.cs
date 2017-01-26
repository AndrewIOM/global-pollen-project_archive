using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core;
using System;
using GlobalPollenProject.Core.Services;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private PollenDbContext _context = null;
        public UnitOfWork(PollenDbContext context)
        {
            _context = context;
        }

        private ITaxonomyBackbone _backboneCoreService = null;
        private IRepository<UnknownGrain> _unknownGrainRepo = null;
        private IRepository<User> _userRepo = null;
        private IRepository<Taxon> _taxonRepo = null;
        private IRepository<ReferenceCollection> _refCollectionRepo = null;
        private IRepository<BackboneTaxonAggregate> _backboneRepo = null;
        private ISlideRepository _slideRepo = null;

        public ITaxonomyBackbone BackboneCoreService
        {
            get
            {
                if (_backboneCoreService == null)
                {
                    if (_backboneRepo == null) _backboneRepo = new KewBackboneTaxonRepository(_context);
                    _backboneCoreService = new LocalKewTaxonomyBackbone(_backboneRepo);
                }
                return _backboneCoreService;
            }
        }

        public IRepository<UnknownGrain> UnknownGrainRepository
        {
            get
            {
                if (_unknownGrainRepo == null) _unknownGrainRepo = new UnknownGrainRepository(_context);
                return _unknownGrainRepo;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (_userRepo == null) _userRepo = new UserRepository(_context);
                return _userRepo;
            }
        }

        public IRepository<Taxon> TaxonRepository
        {
            get
            {
                if (_taxonRepo == null) _taxonRepo = new TaxonRepository(_context);
                return _taxonRepo;
            }
        }

        public IRepository<ReferenceCollection> ReferenceCollectionRepository
        {
            get
            {
                if (_refCollectionRepo == null) _refCollectionRepo = new ReferenceCollectionRepository(_context);
                return _refCollectionRepo;
            }
        }

        public ISlideRepository ReferenceSlideRepository
        {
            get
            {
                if (_slideRepo == null) _slideRepo = new SlideRepository(_context);
                return _slideRepo;
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
