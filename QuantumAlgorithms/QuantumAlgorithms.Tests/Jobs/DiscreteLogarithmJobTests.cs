using System;
using Moq;
using NUnit.Framework;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Jobs;

namespace QuantumAlgorithms.Tests.Jobs
{
    [TestFixture]
    public class DiscreteLogarithmJobTests
    {
        [Test]
        public void RunTest_1()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new DiscreteLogarithmJob(logger.Object, dataService.Object);

            Assert.Throws<InvalidOperationException>(() => job.Run(3, 6, 7));
        }

        [Test]
        public void RunTest_2()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new DiscreteLogarithmJob(logger.Object, dataService.Object);

            var result = job.Run(3, 7, 7);
            Assert.AreEqual(0, result.DiscreteLogarithm);
            Assert.AreEqual(false, result.HadWarnings);
            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        public void RunTest_3()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new DiscreteLogarithmJob(logger.Object, dataService.Object);

            var result = job.Run(3, 3, 7);
            Assert.AreEqual(1, result.DiscreteLogarithm);
            Assert.AreEqual(false, result.HadWarnings);
            Assert.AreEqual(true, result.IsSuccess);
        }

        [Test]
        public void RunTest_4()
        {
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new DiscreteLogarithmJob(logger.Object, dataService.Object);

            var result = job.Run(3, 6, 9);
            Assert.AreEqual(0, result.DiscreteLogarithm);
            Assert.AreEqual(false, result.HadWarnings);
            Assert.AreEqual(false, result.IsSuccess);
        }
    }
}