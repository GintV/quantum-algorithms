using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.JobsService;

namespace QuantumAlgorithms.API.Extensions
{
    public static class EntityExtensions
    {
        public static string StartJobService<TEntity>(this TEntity entity) where TEntity : IEntity =>
            BackgroundJob.Enqueue<IJobService<TEntity>>(jobService => jobService.Execute(entity));
    }
}
