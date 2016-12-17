using System;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITaxonomyBackbone BackboneCoreService { get; }
        
        IRepository<UnknownGrain> UnknownGrainRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Taxon> TaxonRepository { get; }
        IRepository<ReferenceCollection> ReferenceCollectionRepository { get; }
        ISlideRepository ReferenceSlideRepository { get; }

        void SaveChanges();
    }
}
