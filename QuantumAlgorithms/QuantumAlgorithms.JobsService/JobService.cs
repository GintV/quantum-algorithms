using System;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.JobsService
{
    public interface IJobService<TEntity>
        where TEntity : IEntity
    {
        void Execute(TEntity entity);
    }

    public abstract class JobService<TEntity> : IJobService<TEntity>
        where TEntity : IEntity
    {
        protected IDataService<ExecutionMessage, Guid> ExecutionMessageDataService { get; }
        protected IExecutionLogger Logger { get; }

        protected JobService(IDataService<ExecutionMessage, Guid> executionMessageDataService, IExecutionLogger logger)
        {
            ExecutionMessageDataService = executionMessageDataService;
            Logger = logger;
        }

        public abstract void Execute(TEntity entity);
    }
}
