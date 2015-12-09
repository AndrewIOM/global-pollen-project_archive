using OxPollen.Models;
using System;

namespace OxPollen.Data.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRepository<Grain> GrainRepository { get; }
        IRepository<Organisation> OrganisationRepository { get; }
        IRepository<Taxon> TaxonRepository { get; }
        IRepository<GrainImage> ImageRepository { get; }
        IRepository<Identification> IdentificationRepository { get; }
        void SaveChanges();
    }
}
