﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace QuantumAlgorithms.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).
                ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath).
                        AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                        AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).
                        AddEnvironmentVariables();
                }).
                UseStartup<Startup>().
                Build();
    }
}
