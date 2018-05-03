using System;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;

namespace QuantumAlgorithms.JobsService
{
    public class DiscreteLogarithmJobService : JobService<DiscreteLogarithm>
    {
        private readonly IDataService<DiscreteLogarithm, Guid> _discreteLogarithmDataService;

        public DiscreteLogarithmJobService(IDataService<ExecutionMessage, Guid> executionMessageDataService,
            IDataService<DiscreteLogarithm, Guid> discreteLogarithmDataService, IExecutionLogger logger) : base(executionMessageDataService, logger)
        {
            _discreteLogarithmDataService = discreteLogarithmDataService;
        }

        public override void Execute(DiscreteLogarithm entity)
        {
            Logger.SetExecutionId(entity.Id);
            var job = new DiscreteLogarithmJob(Logger);
            var result = job.Run(entity.Generator, entity.Result, entity.Modulus);

            if (result.IsSuccess)
            {
                entity.Exponent = result.DiscreteLogarithm;
                entity.Status = result.HadWarnings ? Status.FinishedWithWarnings : Status.Finished;
            }
            else
            {
                entity.Status = Status.FinishedWithErrors;
            }

            _discreteLogarithmDataService.Update(entity);
            _discreteLogarithmDataService.SaveChanges();
        }
    }
}
