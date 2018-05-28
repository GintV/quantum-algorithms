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
using QuantumAlgorithms.Models.Error;

namespace QuantumAlgorithms.Tests.Models
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void BadRequestDtoTest()
        {
            var dateTime = DateTime.Now;
            var dto = BadRequestDto.InvalidData();

            Assert.AreEqual("400 Bad Request", dto.Error);
            Assert.AreEqual("Model contains fields with invalid values.", dto.ErrorDescription);
        }

        [Test]
        public void NotFoundDtoTest()
        {
            var dto1 = NotFoundDto.ParentNotFound("parentId");
            var dto2 = NotFoundDto.ResourceNotFound("resourceId");

            Assert.AreEqual("404 Not Found", dto1.Error);
            Assert.AreEqual("Parent with ID parentId was not found.", dto1.ErrorDescription);

            Assert.AreEqual("404 Not Found", dto2.Error);
            Assert.AreEqual("Resource with ID resourceId was not found.", dto2.ErrorDescription);
        }
    }
}