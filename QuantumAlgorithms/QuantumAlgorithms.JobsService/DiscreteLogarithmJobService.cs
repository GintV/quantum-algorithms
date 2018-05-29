using System;
using System.Linq;
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

            entity = _discreteLogarithmDataService.Get(entity.Id);
            entity.Status = Status.InProgress;
            _discreteLogarithmDataService.Update(entity);
            _discreteLogarithmDataService.SaveChanges();

            var result = job.Run(entity.Generator, entity.Result, entity.Modulus);

            if (result.IsSuccess)
            {
                entity.Exponent = result.DiscreteLogarithm;
                entity.Status = result.HadWarnings ? Status.FinishedWithWarnings : Status.Finished;
            }
            else
            {
                if (entity.Status != Status.Canceled && !entity.Messages.Last().Message.Contains("canceled"))
                    entity.Status = Status.FinishedWithErrors;
            }

            if (entity.FinishTime == null)
                entity.FinishTime = DateTime.Now;

            //_discreteLogarithmDataService.Update(entity);
            _discreteLogarithmDataService.SaveChanges();
        }
    }
}
