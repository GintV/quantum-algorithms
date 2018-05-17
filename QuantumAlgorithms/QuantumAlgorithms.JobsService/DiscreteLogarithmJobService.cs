using System;
using Hangfire;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;

namespace QuantumAlgorithms.JobsService
{
    public class DiscreteLogarithmJobService : JobService<DiscreteLogarithm>
    {
        private readonly IDataService<DiscreteLogarithm> _discreteLogarithmDataService;

        public DiscreteLogarithmJobService(IDataService<ExecutionMessage> executionMessageDataService,
            IDataService<DiscreteLogarithm> discreteLogarithmDataService, IExecutionLogger logger) : base(executionMessageDataService, logger)
        {
            _discreteLogarithmDataService = discreteLogarithmDataService;
        }

        public override void Execute(DiscreteLogarithm entity)
        {
            Logger.SetExecutionId(entity.Id);
            var job = new DiscreteLogarithmJob(Logger, _discreteLogarithmDataService);

            entity.Status = Status.InProgress;
            _discreteLogarithmDataService.Update(entity);
            _discreteLogarithmDataService.SaveChanges();

            var result = job.Run(entity.Generator, entity.Result, entity.Modulus);

            entity = _discreteLogarithmDataService.Get(entity.Id);
            if (result.IsSuccess)
            {
                entity.Exponent = result.DiscreteLogarithm;
                entity.Status = result.HadWarnings ? Status.FinishedWithWarnings : Status.Finished;
            }
            else
            {
                entity.Status = Status.FinishedWithErrors;
            }
            entity.FinishTime = DateTime.Now;

            //_discreteLogarithmDataService.Update(entity);
            _discreteLogarithmDataService.SaveChanges();
        }
    }
}
