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

namespace QuantumAlgorithms.Tests.API
{
    [TestFixture]
    public class IntegerFactorizationControllerTests
    {
        [Test]
        public void CancelExecutionTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization() {SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new IntegerFactorizationController(dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.CancelExecution(guid);
        }

        [Test]
        public void DeleteResourceTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new IntegerFactorizationController(dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.DeleteResource(guid);
        }

        [Test]
        public void CreateResourceTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new IntegerFactorizationController(dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            Assert.Throws<InvalidOperationException>(() => controller.CreateResource(new IntegerFactorizationCreateDto()));
        }

        [Test]
        public void GetResourceTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new IntegerFactorizationController(dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            Assert.Throws<InvalidOperationException>(() => controller.GetResource(guid));
        }
    }
}