using System;
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

namespace QuantumAlgorithms.Tests.Models
{
    [TestFixture]
    public class CreateTests
    {
        [Test]
        public void DiscreteLogarithmCreateDtoTest()
        {
            var dateTime = DateTime.Now;
            var dto = new DiscreteLogarithmCreateDto { Generator = 1, Modulus = 2, Result = 3, StartTime = dateTime };

            Assert.AreEqual(1, dto.Generator);
            Assert.AreEqual(2, dto.Modulus);
            Assert.AreEqual(3, dto.Result);
            Assert.AreEqual(dateTime, dto.StartTime);
        }

        [Test]
        public void IntegerFactorizationCreateDtoTest()
        {
            var dateTime = DateTime.Now;
            var dto = new IntegerFactorizationCreateDto { Number = 13, StartTime = dateTime };

            Assert.AreEqual(13, dto.Number);
            Assert.AreEqual(dateTime, dto.StartTime);
        }
    }
}