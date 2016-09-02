using Im.Acm.Pollen.Models;
using System;

namespace Im.Acm.Pollen.Data.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRepository<Grain> GrainRepository { get; }
        IRepository<Organisation> OrganisationRepository { get; }
        IRepository<Taxon> TaxonRepository { get; }
        IRepository<GrainImage> ImageRepository { get; }
        IRepository<Identification> IdentificationRepository { get; }
        IRepository<ReferenceCollection> RefCollectionRepository { get; }
        IRepository<ReferenceGrain> RefGrainRepository { get; }
        void SaveChanges();
    }
}
