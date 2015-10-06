using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            PollenDbContext context = (PollenDbContext)serviceProvider.GetService(typeof(PollenDbContext));
            if (context.Database.EnsureCreated())
            {
                if (!context.PollenRecords.Any())
                {
                    var will = context.Users.Add(new ApplicationUser()
                    {
                        UserName = "Will",
                        AccessFailedCount = 0,
                        Email = "will@will.will"
                    }).Entity;

                    context.PollenRecords.Add(new PollenRecord()
                    {
                        ApproximateAge = 0,
                        HasConfirmedIdentity = false,
                        Latitude = 2,
                        Longitude = 57,
                        PhotoUrl = "",
                        PollenRecordId = 1,
                        Taxon = null,
                        User = will
                    });

                    context.Taxa.Add(new Taxon()
                    {
                        CommonName = "European Ash",
                        TaxonId = 1,
                        GbifId = 3172358,
                        LatinName = "Fraxinus excelsior"
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
