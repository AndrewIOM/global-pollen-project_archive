using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using OxPollen.Models;

namespace OxPollen.Data.Concrete
{
    public class OxPollenDbContext : IdentityDbContext<AppUser>
    {
        //User Sumbissions
        public DbSet<Grain> UserGrains { get; set; }
        public DbSet<Identification> Identifications { get; set; }

        //Taxonomy
        public DbSet<Taxon> Taxa { get; set; }

        //Reference Collection
        public DbSet<ReferenceGrain> ReferenceGrains { get; set; }
        public DbSet<ReferenceCollection> ReferenceCollections { get; set; }

        //Shared
        public DbSet<GrainImage> Images { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
    }
}