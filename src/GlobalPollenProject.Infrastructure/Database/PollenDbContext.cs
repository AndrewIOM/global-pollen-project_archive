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

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     foreach (var entity in builder.Model.GetEntityTypes())
        //     {
        //         entity.Relational().TableName = entity.DisplayName();
        //     }

        //     base.OnModelCreating(builder);
        // }

        // private void EnsureRoles(IApplicationBuilder app, ILoggerFactory loggerFactory)
        // {
        //     ILogger logger = loggerFactory.CreateLogger<Startup>();
        //     RoleManager<IdentityRole> roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

        //     string[] roleNames = { "Admin", "Digitise", "Banned" };
        //     foreach (string roleName in roleNames)
        //     {
        //         bool roleExists = roleManager.RoleExistsAsync(roleName).Result;
        //         if (!roleExists)
        //         {
        //             logger.LogInformation(string.Format("!roleExists for roleName {0}", roleName));
        //             IdentityRole identityRole = new IdentityRole(roleName);
        //             IdentityResult identityResult = roleManager.CreateAsync(identityRole).Result;
        //         }
        //     }
        // }

        // private void EnsureAdminUser(IApplicationBuilder app)
        // {
        //     UserManager<AppUser> userManager = app.ApplicationServices.GetService<UserManager<AppUser>>();
        //     var context = app.ApplicationServices.GetService<PollenDbContext>();

        //     var organisation = context.Organisations.FirstOrDefaultAsync(m => m.Name == "Im.Acm.Pollen Admin").Result;
        //     if (organisation == null)
        //     {
        //         organisation = new Organisation()
        //         {
        //             CountryCode = "GB",
        //             Name = "Global Pollen Project Admin"
        //         };
        //         context.Organisations.Add(organisation);
        //         context.SaveChanges();
        //     }

        //     var user = userManager.FindByNameAsync(Configuration["Account:Admin:DefaultAdminUserName"]).Result;
        //     if (user == null)
        //     {
        //         user = new User()
        //         {
        //             UserName = Configuration["Account:Admin:DefaultAdminUserName"],
        //             FirstName = "Im.Acm.Pollen",
        //             LastName = "Admin",
        //             Title = "Mx",
        //             Organisation = organisation,
        //             EmailConfirmed = true,
        //             Email = Configuration["Account:Admin:DefaultAdminUserName"]
        //         };
        //         userManager.CreateAsync(user, Configuration["Account:Admin:DefaultAdminPassword"]).Wait();
        //         userManager.AddToRoleAsync(user, "Admin");
        //     }
        // }
    }
}