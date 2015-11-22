using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace OxPollen.Models
{
    public class OxPollenDbContext : IdentityDbContext<AppUser>
    {
        //Records
        public DbSet<Grain> PollenRecords { get; set; }
        public DbSet<Identification> Identifications { get; set; }

        //Taxonomy
        public DbSet<Taxon> Taxa { get; set; }

        //Users
        public DbSet<Organisation> Organisations { get; set; }
    }
}