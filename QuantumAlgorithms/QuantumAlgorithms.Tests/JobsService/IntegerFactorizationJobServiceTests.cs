using System;
using Moq;
using NUnit.Framework;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;
using QuantumAlgorithms.JobsService;

namespace QuantumAlgorithms.Tests.JobsService
{
    [TestFixture]
    public class IntegerFactorizationJobServiceTests
    {
        [Test]
        public void ExecuteTest_1()
        {
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { Id = guid, SubscriberId = "subject", Status = Status.Enqueued });
            var logger = new Mock<IExecutionLogger>();
            var jobService = new IntegerFactorizationJobService(new Mock<IDataService<ExecutionMessage>>().Object, dataService.Object, logger.Object);

            jobService.Execute(new IntegerFactorization() { Id = guid, Number = 7 });
        }

        [Test]
        public void ExecuteTest_2()
        {
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { Id = guid, SubscriberId = "subject", Status = Status.Enqueued, Number = 8 });
            var logger = new Mock<IExecutionLogger>();
            var jobService = new IntegerFactorizationJobService(new Mock<IDataService<ExecutionMessage>>().Object, dataService.Object, logger.Object);

            jobService.Execute(new IntegerFactorization { Id = guid, Number = 8 });
        }
    }
}