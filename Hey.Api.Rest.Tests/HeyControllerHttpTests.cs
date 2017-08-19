using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Hey.Api.Rest.Models;
using Hey.Api.Rest.Service;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    public class HeyControllerHttpTests
    {
        private HttpConfiguration _httpConfiguration;
        private HttpServer _server;
        private HttpClient _client;
        private string _postUri;
        private HeyRememberDto _heyObj;

        [SetUp]
        public void SetUp()
        {
            _httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(_httpConfiguration);
            WebApiConfig.RegisterDependencies(_httpConfiguration, new Mock<IHeyService>().Object);

            _server = new HttpServer(_httpConfiguration);
            _client = new HttpClient(_server);

            _postUri = "http://localhost/api/Hey";

            _heyObj = new HeyRememberDto()
            {
                Domain = "Test",
                Type = "Post",
                Id = "1",
                When = new[] { DateTime.Now, DateTime.UtcNow }
            };
        }

        [Test]
        public void TestPostJsonAsObject()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _postUri)
            {
                Content = new ObjectContent(typeof(HeyRememberDto), _heyObj, new JsonMediaTypeFormatter())
            };

            using (var response = _client.SendAsync(request))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.Result.StatusCode);
                Assert.AreEqual("http://localhost/api/Hey/1", response.Result.Headers.Location.ToString());
            }
        }

        [Test]
        public void TestPostJsonAsString()
        {
            using (var response = _client.PostAsJsonAsync(_postUri, _heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.Result.StatusCode);
                Assert.AreEqual("http://localhost/api/Hey/1", response.Result.Headers.Location.ToString());
            }
        }
    }
}