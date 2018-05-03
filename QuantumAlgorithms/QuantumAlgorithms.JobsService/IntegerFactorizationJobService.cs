using System;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;

namespace QuantumAlgorithms.JobsService
{
    public class IntegerFactorizationJobService : JobService<IntegerFactorization>
    {
        private readonly IDataService<IntegerFactorization, Guid> _integerFactorizationDataService;

        public IntegerFactorizationJobService(IDataService<ExecutionMessage, Guid> executionMessageDataService,
            IDataService<IntegerFactorization, Guid> integerFactorizationDataService, IExecutionLogger logger) :
            base(executionMessageDataService, logger)
        {
            _integerFactorizationDataService = integerFactorizationDataService;
        }

        public override void Execute(IntegerFactorization entity)
        {
            Logger.SetExecutionId(entity.Id);
            var job = new IntegerFactorizationJob(Logger);
            var result = job.Run(entity.Number);

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

            _integerFactorizationDataService.Update(entity);
            _integerFactorizationDataService.SaveChanges();
        }
    }
}
