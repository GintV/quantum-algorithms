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
    public class DiscreteLogarithmJobServiceTests
    {
        [Test]
        public void ExecuteTest_1()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new DiscreteLogarithm { Id = guid, SubscriberId = "subject", Status = Status.Enqueued });
            var logger = new Mock<IExecutionLogger>();
            var jobService = new DiscreteLogarithmJobService(new Mock<IDataService<ExecutionMessage>>().Object, dataService.Object, logger.Object);

            jobService.Execute(new DiscreteLogarithm { Id = guid, Generator = 3, Result = 6, Modulus = 7 });
        }

        [Test]
        public void ExecuteTest_2()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new DiscreteLogarithm { Id = guid, SubscriberId = "subject", Status = Status.Enqueued, Generator = 3, Result = 3, Modulus = 7 });
            var logger = new Mock<IExecutionLogger>();
            var jobService = new DiscreteLogarithmJobService(new Mock<IDataService<ExecutionMessage>>().Object, dataService.Object, logger.Object);

            jobService.Execute(new DiscreteLogarithm { Id = guid, Generator = 3, Result = 3, Modulus = 7 });
        }
    }
}