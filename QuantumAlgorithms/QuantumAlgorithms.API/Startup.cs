using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.API.Models.Create;
using QuantumAlgorithms.API.Models.Get;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;
using static QuantumAlgorithms.API.Models.Get.IntegerFactorizationGetDto;

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

            //app.UseAuthentication();

            app.UseHangfireDashboard();

            app.UseHangfireServer();

            app.UseMvc();

            //app.UseSwagger();

            //app.UseSwaggerUI(setup =>
            //{
            //    setup.SwaggerEndpoint(SwaggerEndpoint, "CountryClicker API");
            //});
        }
    }
}
