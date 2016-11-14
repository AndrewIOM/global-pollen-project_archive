using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using GlobalPollenProject.Data.Infrastructure;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Infrastructure.Options;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Services;
using GlobalPollenProject.Infrastructure.Storage;
using GlobalPollenProject.Infrastructure;
using GlobalPollenProject.Infrastructure.Communication;
using Microsoft.AspNetCore.Identity;
using GlobalPollenProject.WebUI.Services;
using GlobalPollenProject.Core.Services;

namespace GlobalPollenProject.WebUI
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
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PollenDbContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<User, IdentityRole>()
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
            services.AddApiVersioning( o => o.ReportApiVersions = true );
            services.AddMemoryCache();
            services.AddCors();

            services.AddOptions();
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 99999999999;
            });

            // Infrastructure: Database
            services.Configure<AzureOptions>(Configuration);
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IExternalDatabaseLinker, ExternalDatabaseLinker>();

            // Infrastructure: Other
            services.AddTransient<IImageProcessor, AzureImageProcessor>();
            services.AddTransient<IEmailSender, SendgridEmailSender>();

            // Domain Services
            services.AddTransient<INameConfirmationAlgorithm, SimpleNameConfirmationAlgorithm>();

            // App Services
            services.AddTransient<IDigitisationService, DigitisationService>();
            services.AddTransient<IIdentificationService, IdentificationService>();
            services.AddTransient<ITaxonomyService, TaxonomyService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IUserResolverService, NetCoreUserResolverService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment() || env.IsStaging())
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
            UserManager<User> userManager = app.ApplicationServices.GetService<UserManager<User>>();
            var context = app.ApplicationServices.GetService<PollenDbContext>();

            // var organisation = context.Organisations.FirstOrDefaultAsync(m => m.Name == "Im.Acm.Pollen Admin").Result;
            // if (organisation == null)
            // {
            //     organisation = new Organisation()
            //     {
            //         CountryCode = "GB",
            //         Name = "Global Pollen Project Admin"
            //     };
            //     context.Organisations.Add(organisation);
            //     context.SaveChanges();
            // }

            var user = userManager.FindByNameAsync(Configuration["Account:Admin:DefaultAdminUserName"]).Result;
            if (user == null)
            {
                user = new User("Mx", "GPP", "Admin")
                {
                    UserName = Configuration["Account:Admin:DefaultAdminUserName"],
                    EmailConfirmed = true,
                    Email = Configuration["Account:Admin:DefaultAdminUserName"]
                };
                userManager.CreateAsync(user, Configuration["Account:Admin:DefaultAdminPassword"]).Wait();
                userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
