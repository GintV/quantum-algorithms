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
    public class IntegerFactorizationJobTests
    {
        [Test]
        public void RunTest_1()
        {
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new IntegerFactorizationJob(logger.Object, dataService.Object);

            for (var i = 0; i < 10; ++i)
            {
                try
                {
                    var result = job.Run(15);
                    Assert.AreEqual(15, result.Factors.P * result.Factors.Q);
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        [Test]
        public void RunTest_2()
        {
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var logger = new Mock<IExecutionLogger>();
            var job = new IntegerFactorizationJob(logger.Object, dataService.Object);

            var result = job.Run(8);
            Assert.AreEqual(2, result.Factors.P);
            Assert.AreEqual(4, result.Factors.Q);
            Assert.AreEqual(false, result.HadWarnings);
            Assert.AreEqual(true, result.IsSuccess);
        }
    }
}