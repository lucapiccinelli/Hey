using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class HeyControllerHttpRealTests
    {
        [Ignore("To launch by hand, only if the web service is running")]
        [Test]
        public async Task TestPostJsonOnARealHttpCommunication()
        {
            var client = new HttpClient();
            var heyObj = new HeyRememberDto()
            {
                Domain = "Test",
                Type = "Post",
                Id = "1",
                When = new []{DateTime.Now, DateTime.UtcNow},
                DomainSpecificData = "{TestId: \"idValue\", TestValue: \"banana\"}"
            };

            using (var response = await client.PostAsJsonAsync("http://localhost.fiddler:60402/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.AreEqual("http://localhost:60402/api/Hey/1", response.Headers.Location.ToString());
            }
        }
    }
}
