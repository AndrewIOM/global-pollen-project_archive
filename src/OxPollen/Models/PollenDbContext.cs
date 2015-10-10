using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public class PollenDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PollenRecord> PollenRecords { get; set; }
        public DbSet<Taxon> Taxa { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Institution { get; set; }

        public string FullName
        {
            get
            {
                return Title + " " + FirstName + " " + LastName;
            }
        }
    }
}