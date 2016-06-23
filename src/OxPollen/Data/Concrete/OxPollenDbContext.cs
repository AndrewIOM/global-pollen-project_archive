using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OxPollen.Models;

namespace OxPollen.Data.Concrete
{
    public class OxPollenDbContext : IdentityDbContext<AppUser>
    {
        public OxPollenDbContext(DbContextOptions<OxPollenDbContext> options) : base(options)
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