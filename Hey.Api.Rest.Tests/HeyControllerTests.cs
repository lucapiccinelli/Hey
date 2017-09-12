using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service;
using Hey.Core.Attributes;
using Hey.Core.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    public class HeyControllerTests
    {
        private HeyController _heyController;
        private Mock<IScheduleType> _scheduleTypeMock;
        private Mock<IScheduleTypeFactory> _scheduleTypeFactoryMock;

        [SetUp]
        public void SetUp()
        {
            _scheduleTypeMock = new Mock<IScheduleType>();
            _scheduleTypeMock
                .Setup(type => type.Prototype())
                .Returns(_scheduleTypeMock.Object);
            _scheduleTypeMock
                .Setup(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()))
                .Returns("Mock");

            _scheduleTypeFactoryMock = new Mock<IScheduleTypeFactory>();
            _scheduleTypeFactoryMock
                .Setup(factory => factory.MakeAPrototype(It.IsAny<HeyRememberDto>()))
                .Returns(_scheduleTypeMock.Object);

            _heyController = new HeyController(new HeyService(_scheduleTypeFactoryMock.Object, new LogExceptionHandler()));
        }

        [Test]
        public void TestPostCantFindTheAssembly()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Banana"
            };
            var response = _heyController.Post(heyObj);
            Assert.IsInstanceOf<ExceptionResult>(response);
            _scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()), Times.Never);
        }

        [Test]
        public void TestPostCantFindTheNamespace()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest",
                Type = "Banana"
            };

            var response = _heyController.Post(heyObj);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
            _scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()), Times.Never);
        }

        [Test]
        public void TestPostCantFindTheMethod()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest",
                Type = "Tests",
                Id = "Banana"
            };

            var response = _heyController.Post(heyObj);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
            _scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()), Times.Never);
        }

        [Test]
        public void TestPostCanFindTheMethodButHangFireIsNotRunning()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Id = "Test",
                DomainSpecificData = $"[{JsonConvert.SerializeObject(DateTime.Now)}, \"abc\"]"
            };

            var scheduleTypeMock = new Mock<IScheduleType>();
            scheduleTypeMock
                .Setup(type => type.Prototype())
                .Returns(scheduleTypeMock.Object);
            scheduleTypeMock
                .Setup(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()))
                .Throws<Exception>();

            var scheduleTypeFactoryMock = new Mock<IScheduleTypeFactory>();
            scheduleTypeFactoryMock
                .Setup(factory => factory.MakeAPrototype(It.IsAny<HeyRememberDto>()))
                .Returns(scheduleTypeMock.Object);

            var heyController = new HeyController(new HeyService(scheduleTypeFactoryMock.Object, new LogExceptionHandler()));
            var response = heyController.Post(heyObj);
            Assert.IsInstanceOf<ExceptionResult>(response);
            scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()));
        }

        [Test]
        public void TestPostCanFindTheMethod()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Id = "Test",
                DomainSpecificData = $"[{JsonConvert.SerializeObject(DateTime.Now)}, \"abc\"]"
            };

            var response = _heyController.Post(heyObj);
            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<HeyRememberDto>>(response);
            _scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()));
        }

        [Test]
        public void TestPostCantMatchMethodArguments()
        {
            HeyRememberDto heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Api.Rest.Tests",
                Id = "Test",
                DomainSpecificData = $"[{JsonConvert.SerializeObject(DateTime.Now)}, 1]"
            };

            var response = _heyController.Post(heyObj);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
            _scheduleTypeMock.Verify(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()), Times.Never);
        }
    }

    public class HttpTestClass
    {
        [FireMe("Test")]
        public void FindMeMethod(DateTime d, string s) { }
    }
}