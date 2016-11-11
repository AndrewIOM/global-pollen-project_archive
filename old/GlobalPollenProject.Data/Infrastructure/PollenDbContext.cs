using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class PollenDbContext : IdentityDbContext<AppUser>
    {
        public PollenDbContext(DbContextOptions<PollenDbContext> options) : base(options)
        {
        }

        //User Sumbissions
        public DbSet<Grain> UserGrains { get; set; }
        public DbSet<Identification> Identifications { get; set; }

        //Taxonomy
        public DbSet<Taxon> Taxa { get; set; }
        public DbSet<PlantListTaxon> PlantListTaxa { get; set; }

        //Reference Collection
        public DbSet<ReferenceGrain> ReferenceGrains { get; set; }
        public DbSet<ReferenceCollection> ReferenceCollections { get; set; }

        //Shared
        public DbSet<GrainImage> Images { get; set; }
        public DbSet<Organisation> Organisations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            base.OnModelCreating(builder);
        }
    }
}