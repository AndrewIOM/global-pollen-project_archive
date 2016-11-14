using GlobalPollenProject.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class PollenDbContext : IdentityDbContext<User>
    {
        public DbSet<UnknownGrain> UnknownGrains { get; set; }
        public DbSet<ReferenceCollection> ReferenceCollections { get; set; }
        public DbSet<Taxon> Taxa { get; set; }
        public DbSet<KewBackboneTaxon> BackboneTaxa { get; set; }

        public PollenDbContext(DbContextOptions<PollenDbContext> options)
            : base(options)
        { }
    }
}