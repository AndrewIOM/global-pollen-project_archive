using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace OxPollen.Models
{
    public class OxPollenDbContext : IdentityDbContext<AppUser>
    {
        //Records
        public DbSet<Grain> UserGrains { get; set; }
        public DbSet<Identification> Identifications { get; set; }
        public DbSet<GrainImage> Images { get; set; }

        //Taxonomy
        public DbSet<Taxon> Taxa { get; set; }
        public DbSet<ReferenceGrain> ReferenceGrains { get; set; }

        //Users
        public DbSet<Organisation> Organisations { get; set; }
    }
}