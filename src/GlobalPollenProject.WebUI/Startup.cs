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
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            }
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

            // Infrastructure: Other
            services.AddTransient<IImageProcessor, AzureImageProcessor>();
            services.AddTransient<IEmailSender, SendgridEmailSender>();

            // App Services
            services.AddTransient<IDigitisationService, DigitisationService>();
            services.AddTransient<IIdentificationService, IdentificationService>();
            services.AddTransient<ITaxonomyService, TaxonomyService>();
            services.AddTransient<IUserService, UserService>();
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
        }
    }
}
