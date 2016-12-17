using GlobalPollenProject.Core;
using GlobalPollenProject.Core.State;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class PollenDbContext : IdentityDbContext<User>
    {
        public DbSet<UnknownGrainState> UnknownGrains { get; set; }
        public DbSet<ReferenceCollectionState> ReferenceCollections { get; set; }
        public DbSet<SlideState> Slides { get; set; }
        public DbSet<TaxonState> Taxa { get; set; }
        public DbSet<BackboneTaxonState> BackboneTaxa { get; set; }

        public PollenDbContext(DbContextOptions<PollenDbContext> options)
            : base(options)
        { }
    }
}