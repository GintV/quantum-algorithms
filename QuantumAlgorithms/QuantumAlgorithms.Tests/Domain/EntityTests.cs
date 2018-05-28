using System;
using NUnit.Framework;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Tests.Domain
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void DiscreteLogarithmTest()
        {
            var dateTime = DateTime.Now;
            var guid = Guid.NewGuid();
            var entity = new DiscreteLogarithm
            {
                CancelJob = true,
                Exponent = 1,
                FinishTime = dateTime,
                Generator = 2,
                Id = guid,
                InnerJobId = "inner",
                JobId = "main",
                Messages = null,
                Modulus = 3,
                Result = 4,
                StartTime = dateTime,
                Status = Status.Canceled,
                SubscriberId = "sub"
            };
            

            Assert.AreEqual(true, entity.CancelJob);
            Assert.AreEqual(1, entity.Exponent);
            Assert.AreEqual(dateTime, entity.FinishTime);
            Assert.AreEqual(2, entity.Generator);
            Assert.AreEqual(guid, entity.Id);
            Assert.AreEqual("inner", entity.InnerJobId);
            Assert.AreEqual("main", entity.JobId);
            Assert.AreEqual(null, entity.Messages);
            Assert.AreEqual(3, entity.Modulus);
            Assert.AreEqual(4, entity.Result);
            Assert.AreEqual(dateTime, entity.StartTime);
            Assert.AreEqual(Status.Canceled, entity.Status);
            Assert.AreEqual("sub", entity.SubscriberId);
        }

        [Test]
        public void IntegerFactorizationTest()
        {
            var dateTime = DateTime.Now;
            var guid = Guid.NewGuid();
            var entity = new IntegerFactorization
            {
                CancelJob = true,
                Number = 11,
                FinishTime = dateTime,
                FactorP = 22,
                Id = guid,
                InnerJobId = "inner",
                JobId = "main",
                Messages = null,
                FactorQ = 33,
                StartTime = dateTime,
                Status = Status.Canceled,
                SubscriberId = "sub"
            };


            Assert.AreEqual(true, entity.CancelJob);
            Assert.AreEqual(11, entity.Number);
            Assert.AreEqual(dateTime, entity.FinishTime);
            Assert.AreEqual(22, entity.FactorP);
            Assert.AreEqual(guid, entity.Id);
            Assert.AreEqual("inner", entity.InnerJobId);
            Assert.AreEqual("main", entity.JobId);
            Assert.AreEqual(null, entity.Messages);
            Assert.AreEqual(33, entity.FactorQ);
            Assert.AreEqual(dateTime, entity.StartTime);
            Assert.AreEqual(Status.Canceled, entity.Status);
            Assert.AreEqual("sub", entity.SubscriberId);
        }

        [Test]
        public void ExecutionMessageTest()
        {
            var dateTime = DateTime.Now;
            var guid = Guid.NewGuid();
            var entity = new ExecutionMessage
            {
                Id = guid,
                Message = "hello",
                QuantumAlgorithm = null,
                QuantumAlgorithmId = guid,
                Severity = ExecutionMessageSeverity.Warning,
                TimeStamp = dateTime
            };


            Assert.AreEqual(guid, entity.Id);
            Assert.AreEqual("hello", entity.Message);
            Assert.AreEqual(null, entity.QuantumAlgorithm);
            Assert.AreEqual(guid, entity.QuantumAlgorithmId);
            Assert.AreEqual(ExecutionMessageSeverity.Warning, entity.Severity);
            Assert.AreEqual(dateTime, entity.TimeStamp);
        }
    }
}