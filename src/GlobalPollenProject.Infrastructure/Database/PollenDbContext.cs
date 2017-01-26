using System.Linq;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Infrastructure.EventHandlers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class PollenDbContext : IdentityDbContext<UserState>
    {
        public PollenDbContext(DbContextOptions<PollenDbContext> options, IDomainEventDispatcher dispatcher) : base(options) 
        {
            _dispatcher = dispatcher;
        }

        // Root Aggregates in Domain (State Objects)
        public DbSet<UnknownGrainState> UnknownGrains { get; set; }
        public DbSet<ReferenceCollectionState> ReferenceCollections { get; set; }
        public DbSet<TaxonState> Taxa { get; set; }
        public DbSet<BackboneTaxonState> BackboneTaxa { get; set; }


        // Handle Domain Events
        private IDomainEventDispatcher _dispatcher;

        public override int SaveChanges() {
            var domainEventEntities = ChangeTracker.Entries<IEntity>()
                .Select(po => po.Entity)
                .Where(po => po.Events.Any())
                .ToArray();

            foreach (var entity in domainEventEntities)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return base.SaveChanges();
        }

    }
    
}