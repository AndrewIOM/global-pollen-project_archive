using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using OxPollen.Data.Abstract;
using OxPollen.Data.Concrete;
using OxPollen.Models;
using OxPollen.Services;
using OxPollen.Services.Abstract;
using OxPollen.Services.Concrete;
using System;
using System.Threading.Tasks;

namespace OxPollen
{
    public class Startup
    {
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Entity Framework services to the services container.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<OxPollenDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // Add Identity services to the services container.
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<OxPollenDbContext>()
                .AddDefaultTokenProviders();

            // Add MVC services to the services container.
            services.AddMvc();

            //Add CORS
            services.AddCors();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            // Register application services.
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IIdentificationService, IdentificationService>();
            services.AddTransient<IGrainService, GrainService>();
            services.AddTransient<IFileStoreService, ImageService>();
            services.AddTransient<IReferenceService, ReferenceService>();
            services.AddTransient<ITaxonomyService, TaxonomyService>();

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
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage(options =>
                {
                    options.EnableAll();
                });
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            // Add and configure the options for authentication middleware to the request pipeline.
            // You can add options for middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            //app.UseFacebookAuthentication(options =>
            //{
            //    options.AppId = Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //});
            //app.UseGoogleAuthentication(options =>
            //{
            //    options.ClientId = Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});
            app.UseMicrosoftAccountAuthentication(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });
            //app.UseTwitterAuthentication(options =>
            //{
            //    options.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
            //    options.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            //});

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                //routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

            //Custom
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
            var context = app.ApplicationServices.GetService<OxPollenDbContext>();

            var organisation = context.Organisations.FirstOrDefaultAsync(m => m.Name == "OxPollen Admin").Result;
            if (organisation == null)
            {
                organisation = new Organisation()
                {
                    CountryCode = "GB",
                    Name = "OxPollen Admin"
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
                    FirstName = "OxPollen",
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
