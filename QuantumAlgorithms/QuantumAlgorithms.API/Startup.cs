using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.API.HangfireAuthorization;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Get;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;
using Swashbuckle.AspNetCore.Swagger;
using static QuantumAlgorithms.API.Constants;
using static QuantumAlgorithms.Models.Get.IntegerFactorizationGetDto;

namespace QuantumAlgorithms.API
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
            var clientUrl = Configuration.GetSection("Applications")["Client"];
            clientUrl = clientUrl.Substring(0, clientUrl.Length - 1);

            services.AddCors(setup => setup.AddPolicy("AllowMyClient", configure =>
            {
                configure.WithOrigins(clientUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
            }));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme).AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration.GetSection("Applications")["IDP"];
                options.RequireHttpsMetadata = true;
                options.ApiName = "qsolverapi";
                options.ApiSecret = "qsolverapisecret";
            })/*.AddCookie("Cookies", options =>
            {
                options.AccessDeniedPath = "/Authorization/AccessDenied";
            }).AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = Configuration.GetSection("Applications")["IDP"];
                options.RequireHttpsMetadata = true;
                options.ClientId = "qsolverapi";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                //options.Scope.Add("address");
                //options.Scope.Add("roles");
                //options.Scope.Add("courses");
                //options.Scope.Add("qsolverapi");
                //options.Scope.Add("nickname");
                //options.Scope.Add("offline_access");
                //options.ClaimActions.MapUniqueJsonKey("nickname", "nickname");
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.ClientSecret = "qsolverapisecret";
                options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
                {
                    OnTicketReceived = ticketReceivedContext =>
                    {
                        return Task.CompletedTask;
                    },

                    OnTokenValidated = tokenValidatedContext =>
                    {
                        var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;

                        var targetClaims = identity.Claims.Where(z => new[] { "roles", "sub", "nickname", "email" }.Contains(z.Type));
                        var newClaimsIdentity = new ClaimsIdentity(
                            identity.Claims,
                            identity.AuthenticationType,
                            "given_name",
                            "roles");

                        tokenValidatedContext.Principal = new ClaimsPrincipal(newClaimsIdentity);

                        return Task.CompletedTask;
                    },

                    OnUserInformationReceived = userInformationReceivedContext =>
                    {
                        userInformationReceivedContext.User.Remove("address");
                        return Task.FromResult(0);
                    }
                };
            })*/;

            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"));
            });

            services.AddMvc();

            services.AddDbContext<QuantumAlgorithmsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("QuantumAlgorithms"));
            });

            services.AddDataServices();

            services.AddJobsServices();

            services.AddDriversServices();

            services.AddScoped<IExecutionLogger, ExecutionLogger>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(ApiVersionString, new Info { Title = "QuantumAlgorithms API", Version = ApiVersionString });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            AutoMapper.Mapper.Initialize(configuration => configuration.SetupAutoMapper());

            app.UseCors("AllowMyClient");

            app.UseAuthentication();

            app.UseMvc();

            app.UseHangfireDashboard( /*options: new DashboardOptions
            {
                Authorization = new[]
                {
                    new AuthorizationFilter(Configuration)
                }
            }*/);

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = "JobServiceServer",
                WorkerCount = 15,
                Queues = new[] { "job" }
            });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = "DriverServiceServer",
                WorkerCount = 10,
                Queues = new[] { "driver" }
            });

            app.UseSwagger();

            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint(SwaggerEndpoint, "QuantumAlgorithms API");
            });
        }
    }
}
