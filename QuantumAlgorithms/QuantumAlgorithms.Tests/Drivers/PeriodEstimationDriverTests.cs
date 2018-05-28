using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NUnit.Framework;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Drivers.PeriodEstimation;

namespace QuantumAlgorithms.Tests.Drivers
{
    [TestFixture]
    public class PeriodEstimationDriverTests
    {
        [Test]
        public void RunTest()
        {
            var logger = new Mock<IExecutionLogger>();
            var driver = new PeriodEstimationDriver(logger.Object);

            driver.Run(3, 7);
        }
    }
}
