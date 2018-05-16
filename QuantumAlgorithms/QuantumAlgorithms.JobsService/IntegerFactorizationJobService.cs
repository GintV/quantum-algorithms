using System;
using Hangfire;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;

namespace QuantumAlgorithms.JobsService
{
    public class IntegerFactorizationJobService : JobService<IntegerFactorization>
    {
        private readonly IDataService<IntegerFactorization> _integerFactorizationDataService;

        public IntegerFactorizationJobService(IDataService<ExecutionMessage> executionMessageDataService,
            IDataService<IntegerFactorization> integerFactorizationDataService, IExecutionLogger logger) :
            base(executionMessageDataService, logger)
        {
            _integerFactorizationDataService = integerFactorizationDataService;
        }

        public override void Execute(IntegerFactorization entity)
        {
            Logger.SetExecutionId(entity.Id);
            var job = new IntegerFactorizationJob(Logger, _integerFactorizationDataService);
            var result = job.Run(entity.Number);

            entity = _integerFactorizationDataService.Get(entity.Id);
            if (result.IsSuccess)
            {
                entity.FactorP = result.Factors.P;
                entity.FactorQ = result.Factors.Q;
                entity.Status = result.HadWarnings ? Status.FinishedWithWarnings : Status.Finished;
            }
            else
            {
                entity.Status = Status.FinishedWithErrors;
            }
            entity.FinishTime = DateTime.Now;

            _integerFactorizationDataService.Update(entity);
            _integerFactorizationDataService.SaveChanges();
        }
    }
}
