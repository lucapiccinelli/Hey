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
        public async Task TestThatWhenTheJobIsScheduledItIsReturnedTheCorrectHttpPath()
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

            HeyController controller = new HeyController(new Mock<IHeyService>().Object);
            controller.Request = _httpRequestMessage;
            controller.Configuration = _httpConfiguration;

            OkHeyResponse response = new OkHeyResponse(methodBinderMock.Object, scheduleTypeMock.Object);
            IHttpActionResult result = response.Execute(controller);

            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<HeyRememberDto>>(result);

            UrlHelper urlHelper = new UrlHelper(_httpRequestMessage);

            using (var httpResponse = await result.ExecuteAsync(CancellationToken.None))
            {
                Assert.AreEqual(
                    "http://localhost:60402/api/Hey/TestDomain/TestType/TestId/banana",
                    httpResponse.Headers.Location.ToString());
            }

            //var concreteResult = (CreatedAtRouteNegotiatedContentResult<HeyRememberDto>)result;
            //Assert.AreEqual(
            //    "http://localhost:60402/api/Hey/TestDomain/TestType/TestId/banana",
            //    urlHelper.Link(concreteResult.RouteName, concreteResult.RouteValues));
        }
    }
}
