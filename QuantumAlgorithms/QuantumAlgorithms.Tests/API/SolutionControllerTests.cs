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
    public class SolutionControllerTests
    {
        [Test]
        public void CancelExecutionTest_1()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization() {SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(new Mock<IDataService<DiscreteLogarithm>>().Object, dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.CancelExecution(guid);
        }
        [Test]
        public void CancelExecutionTest_2()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new DiscreteLogarithm() { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(dataService.Object, new Mock<IDataService<IntegerFactorization>>().Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.CancelExecution(guid);
        }

        [Test]
        public void DeleteResourceTest_1()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(new Mock<IDataService<DiscreteLogarithm>>().Object, dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.DeleteResource(guid);
        }

        [Test]
        public void DeleteResourceTest_2()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new DiscreteLogarithm() { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(dataService.Object, new Mock<IDataService<IntegerFactorization>>().Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            controller.DeleteResource(guid);
        }

        [Test]
        public void GetResourceTest_1()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<IntegerFactorization>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new IntegerFactorization { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(new Mock<IDataService<DiscreteLogarithm>>().Object, dataService.Object,
                new Mock<IExecutionLogger>().Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            Assert.NotNull(controller);

            Assert.Throws<InvalidOperationException>(() => controller.GetResource(guid));
        }
        [Test]
        public void GetResourceTest_2()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "subject"),
            }));
            var dataService = new Mock<IDataService<DiscreteLogarithm>>();
            var guid = Guid.NewGuid();
            dataService.Setup(self => self.Get(guid)).Returns(() => new DiscreteLogarithm() { SubscriberId = "subject", Status = Status.Enqueued });
            var controller = new SolutionController(dataService.Object, new Mock<IDataService<IntegerFactorization>>().Object,
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