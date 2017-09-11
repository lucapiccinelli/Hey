using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Response;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Hey.Api.Rest.Tests
{
    [TestFixture()]
    class OkHeyResponseTests
    {

        private HttpConfiguration _httpConfiguration;
        private HttpServer _server;
        private HttpClient _client;
        private string _postUri;
        private HeyRememberDto _heyObj;
        private HttpRequestMessage _httpRequestMessage;

        [SetUp]
        public void SetUp()
        {
            _httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(_httpConfiguration);

            _postUri = "http://localhost/api/Hey";

            _heyObj = new HeyRememberDto()
            {
                Domain = "TestDomain",
                Type = "TestType",
                Id = "TestId",
            };

            _httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _postUri)
            {
                Content = new ObjectContent(typeof(HeyRememberDto), _heyObj, new JsonMediaTypeFormatter())
            };
        }

        [Test]
        public void TestThatWhenTheJobIsScheduledItIsReturnedTheCorrectHttpPath()
        {
            Mock<IMethodBinder> methodBinderMock = new Mock<IMethodBinder>();
            methodBinderMock
                .Setup(binder => binder.CreateDeferredExecution())
                .Returns(new HeyRememberDeferredExecution()
                {
                    HeyRemember = _heyObj
                });

            Mock<IScheduleType> scheduleTypeMock = new Mock<IScheduleType>();
            scheduleTypeMock
                .Setup(type => type.Schedule(It.IsAny<HeyRememberDeferredExecution>()))
                .Returns("banana");

            OkHeyResponse response = new OkHeyResponse(methodBinderMock.Object, scheduleTypeMock.Object);
            var serviceMock = new Mock<IHeyService>();
            serviceMock
                .Setup(service => service.Handle(_heyObj))
                .Returns(response);

            HeyController controller = new HeyController(serviceMock.Object)
            {
                Request = _httpRequestMessage,
                Configuration = _httpConfiguration
            };
            IHttpActionResult result = controller.Post(_heyObj);

            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<HeyRememberDto>>(result);
            var createdAtResult = result as CreatedAtRouteNegotiatedContentResult<HeyRememberDto>;

            Assert.AreEqual("DefaultApi", createdAtResult.RouteName);
            Assert.AreEqual("TestDomain/TestType/TestId/banana", createdAtResult.RouteValues["id"]);
        }
    }
}
