using System;
using Microsoft.Extensions.DependencyInjection;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Drivers.PeriodEstimation;
using QuantumAlgorithms.DriversService;
using QuantumAlgorithms.JobsService;

namespace QuantumAlgorithms.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataServices(this IServiceCollection services) => services.
            AddScoped<IDataService<DiscreteLogarithm, Guid>, DiscreteLogarithmDataService>().
            AddScoped<IDataService<ExecutionMessage, Guid>, ExecutionMessageDataService>().
            AddScoped<IDataService<IntegerFactorization, Guid>, IntegerFactorizationDataService>();

        public static void AddJobsServices(this IServiceCollection services) => services.
            AddScoped<IJobService<DiscreteLogarithm>, DiscreteLogarithmJobService>().
            AddScoped<IJobService<IntegerFactorization>, IntegerFactorizationJobService>();

        public static void AddDriversServices(this IServiceCollection services) => services.
            AddScoped<IDriverService<PeriodEstimationDriverInput, int>, PeriodEstimationDriverService>();
    }
}
