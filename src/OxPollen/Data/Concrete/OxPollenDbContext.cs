using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using OxPollen.Models;

namespace OxPollen.Data.Concrete
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