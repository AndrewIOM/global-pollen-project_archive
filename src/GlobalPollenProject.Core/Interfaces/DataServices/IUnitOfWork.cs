using System;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core.Interfaces
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
        IRepository<PlantListTaxon> TaxonBackboneRepository { get; }
        void SaveChanges();
    }
}
