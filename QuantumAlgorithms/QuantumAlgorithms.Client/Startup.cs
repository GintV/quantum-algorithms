using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuantumAlgorithms.Client.Api.Services;

namespace QuantumAlgorithms.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization();

            // Register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register an IQuantumAlgorithmHttpClient
            services.AddScoped<IQuantumAlgorithmsHttpClient, QuantumAlgorithmsHttpClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", options =>
            {
                options.AccessDeniedPath = "/Authorization/AccessDenied";
            }).AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = Configuration.GetSection("Applications")["IDP"];
                options.RequireHttpsMetadata = true;
                options.ClientId = "qsolverclient";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                //options.Scope.Add("address");
                options.Scope.Add("roles");
                //options.Scope.Add("courses");
                //options.Scope.Add("imagegalleryapi");
                options.Scope.Add("qsolverapi");
                //options.Scope.Add("nickname");
                //options.Scope.Add("subscriptionlevel");
                // options.Scope.Add("country");
                options.Scope.Add("offline_access");
                //options.ClaimActions.MapUniqueJsonKey("nickname", "nickname");
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.ClientSecret = "qsolverclientsecret";
                options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
                {
                    OnTicketReceived = ticketReceivedContext =>
                    {
                        return Task.CompletedTask;
                    },

                    OnTokenValidated = tokenValidatedContext =>
                    {
                        var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;

                        var targetClaims = identity.Claims.Where(z => new[] { "subscriptionlevel", "country", "role", "sub", "nickname", "email" }.Contains(z.Type));
                        var newClaimsIdentity = new ClaimsIdentity(
                            identity.Claims,
                            identity.AuthenticationType,
                            "given_name",
                            "role");

                        tokenValidatedContext.Principal = new ClaimsPrincipal(newClaimsIdentity);

                        return Task.CompletedTask;
                    },

                    OnUserInformationReceived = userInformationReceivedContext =>
                    {
                        userInformationReceivedContext.User.Remove("address");
                        return Task.FromResult(0);
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
