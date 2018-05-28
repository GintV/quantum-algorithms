using System;
using Moq;
using NUnit.Framework;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Drivers.PeriodEstimation;
using QuantumAlgorithms.DriversService;
using QuantumAlgorithms.JobsService;

namespace QuantumAlgorithms.Tests.DriversService
{
    [TestFixture]
    public class PeriodEstimationDriverServiceTests
    {
        [Test]
        public void RunTest()
        {
            var guid = Guid.NewGuid();
            var logger = new Mock<IExecutionLogger>();
            var driverService = new PeriodEstimationDriverService(logger.Object);

            driverService.Run(new PeriodEstimationDriverInput { ExecutionId = guid, Number = 3, Modulus = 7 });
        }
    }
}