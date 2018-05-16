using System.Reflection;
using Ginsoft.IDP.Entities;
using Ginsoft.IDP.Services;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.DbContexts;

namespace Ginsoft.IDP
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var store = Configuration.GetConnectionString("GinsoftUserDB");

            services.AddDbContext<GinsoftUserContext>(options =>
            {
                options.UseSqlServer(store, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            });

            services.AddScoped<IGinsoftUserRepository, GinsoftUserRepository>();

            //services.AddDbContext<GinsoftUserContext>(builder =>
            //    builder.UseSqlServer(store, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<GinsoftUserContext>();

            services.AddMvc();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddGinsoftUserStore()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(Configuration.GetSection("Applications")))
                /*.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(store,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })*/
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(store,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                });

            services.AddAuthentication().AddFacebook("Facebook", "Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.AppId = "195656230995298";
                options.AppSecret = "6f7f714ae0b17bb766f18d8b683a541c";
            }).AddCookie("idsrv.2FA");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            PersistedGrantDbContext persistedGrantDbContext, /*ConfigurationDbContext configurationDbContext,*/ GinsoftUserContext ginsoftUserContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            persistedGrantDbContext.Database.Migrate();
            //configurationDbContext.Database.Migrate();
            ginsoftUserContext.Database.Migrate();
            //ginsoftUserContext.EnsureSeedDataForContext();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
