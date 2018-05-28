using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using QuantumAlgorithms.API.Controllers;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Error;
using QuantumAlgorithms.Models.Get;

namespace QuantumAlgorithms.Tests.Models
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public void DiscreteLogarithmGetDtoTest()
        {
            var dateTime = DateTime.Now;
            var guid = Guid.NewGuid();
            var dto = new DiscreteLogarithmGetDto
            {
                FinishTime = null,
                Id = guid,
                Input = new DiscreteLogarithmInputDto { Generator = 1, Modulus = 2, Result = 3 },
                Messages = new[]
                {
                    new ExecutionMessageDto
                    {
                        Message = "message",
                        Severity = ExecutionMessageSeverity.Error,
                        TimeStamp = dateTime
                    }
                },
                Output = new DiscreteLogarithmOutputDto { DiscreteLogarithm = 4 },
                StartTime = dateTime,
                Status = Status.InProgress
            };

            Assert.AreEqual(null, dto.FinishTime);
            Assert.AreEqual(guid, dto.Id);
            Assert.AreEqual(1, dto.Input.Generator);
            Assert.AreEqual(2, dto.Input.Modulus);
            Assert.AreEqual(3, dto.Input.Result);
            Assert.AreEqual("message", dto.Messages.First().Message);
            Assert.AreEqual(ExecutionMessageSeverity.Error, dto.Messages.First().Severity);
            Assert.AreEqual(dateTime, dto.Messages.First().TimeStamp);
            Assert.AreEqual(4, dto.Output.DiscreteLogarithm);
            Assert.AreEqual(dateTime, dto.StartTime);
            Assert.AreEqual(Status.InProgress, dto.Status);
            Assert.AreEqual("In Progress", dto.StatusString);
        }

        [Test]
        public void IntegerFactorizationGetDtoTest()
        {
            var dateTime = DateTime.Now;
            var guid = Guid.NewGuid();
            var dto = new IntegerFactorizationGetDto
            {
                FinishTime = null,
                Id = guid,
                Input = new IntegerFactorizationInputDto { Number = 4 },
                Messages = new[]
                {
                    new ExecutionMessageDto
                    {
                        Message = "message",
                        Severity = ExecutionMessageSeverity.Error,
                        TimeStamp = dateTime
                    }
                },
                Output = new IntegerFactorizationOutputDto { P = 7, Q = 9 },
                StartTime = dateTime,
                Status = Status.InProgress
            };

            Assert.AreEqual(null, dto.FinishTime);
            Assert.AreEqual(guid, dto.Id);
            Assert.AreEqual(4, dto.Input.Number);
            Assert.AreEqual("message", dto.Messages.First().Message);
            Assert.AreEqual(ExecutionMessageSeverity.Error, dto.Messages.First().Severity);
            Assert.AreEqual(dateTime, dto.Messages.First().TimeStamp);
            Assert.AreEqual(7, dto.Output.P);
            Assert.AreEqual(9, dto.Output.Q);
            Assert.AreEqual(dateTime, dto.StartTime);
            Assert.AreEqual(Status.InProgress, dto.Status);
            Assert.AreEqual("In Progress", dto.StatusString);
        }
    }
}