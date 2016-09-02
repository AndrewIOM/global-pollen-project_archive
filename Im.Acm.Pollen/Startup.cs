using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Im.Acm.Pollen.Data.Abstract;
using Im.Acm.Pollen.Data.Concrete;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.Services;
using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.Services.Concrete;
using System.IO;
using Im.Acm.Pollen.Options;
using Microsoft.AspNetCore.Http.Features;

namespace Im.Acm.Pollen
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

            host.Run();
        }

        public Startup(IHostingEnvironment env, IHostingEnvironment hostEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PollenDbContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<PollenDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("ReferenceCollectionHolders", policy =>
                {
                    policy.RequireRole("Digitise");
                });
            });

            services.AddMvc();
            services.AddMemoryCache();
            services.AddCors();

            //Options
            services.AddOptions();
            services.Configure<AzureOptions>(Configuration);
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 99999999999;
            });

            // DI Services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IIdentificationService, IdentificationService>();
            services.AddTransient<IGrainService, GrainService>();
            services.AddTransient<IFileStoreService, AzureImageService>();
            services.AddTransient<IReferenceService, ReferenceService>();
            services.AddTransient<ITaxonomyService, TaxonomyService>();
            services.AddTransient<ITaxonomyBackbone, LocalPlantListTaxonomyBackbone>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });
            app.UseTwitterAuthentication(new TwitterOptions()
            {
                ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"],
                ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"]
            });

            //Setup CORS
            app.UseCors(builder =>
                builder.WithOrigins("http://api.gbif.org", "http://api.neotomadb.org")
                .AllowAnyHeader()
            );


            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            EnsureRoles(app, loggerFactory);
            EnsureAdminUser(app);
        }

        private void EnsureRoles(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger<Startup>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "Digitise", "Banned" };
            foreach (string roleName in roleNames)
            {
                bool roleExists = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExists)
                {
                    logger.LogInformation(string.Format("!roleExists for roleName {0}", roleName));
                    IdentityRole identityRole = new IdentityRole(roleName);
                    IdentityResult identityResult = roleManager.CreateAsync(identityRole).Result;
                }
            }
        }

        private void EnsureAdminUser(IApplicationBuilder app)
        {
            UserManager<AppUser> userManager = app.ApplicationServices.GetService<UserManager<AppUser>>();
            var context = app.ApplicationServices.GetService<PollenDbContext>();

            var organisation = context.Organisations.FirstOrDefaultAsync(m => m.Name == "Im.Acm.Pollen Admin").Result;
            if (organisation == null)
            {
                organisation = new Organisation()
                {
                    CountryCode = "GB",
                    Name = "Im.Acm.Pollen Admin"
                };
                context.Organisations.Add(organisation);
                context.SaveChanges();
            }

            var user = userManager.FindByNameAsync(Configuration["Account:Admin:DefaultAdminUserName"]).Result;
            if (user == null)
            {
                user = new AppUser()
                {
                    UserName = Configuration["Account:Admin:DefaultAdminUserName"],
                    FirstName = "Im.Acm.Pollen",
                    LastName = "Admin",
                    Title = "Mx",
                    Organisation = organisation,
                    EmailConfirmed = true,
                    Email = Configuration["Account:Admin:DefaultAdminUserName"]
                };
                userManager.CreateAsync(user, Configuration["Account:Admin:DefaultAdminPassword"]).Wait();
                userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
