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
                Domain = "Hey.Soardi",
                Type = "Mail",
                Id = "Note",
                When = new []{DateTime.Now, DateTime.UtcNow},
                DomainSpecificData = "[1, \"banana\"]"
            };

            using (var response = await client.PostAsJsonAsync("http://localhost:60402/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.AreEqual("http://localhost:60402/api/Hey/Note", response.Headers.Location.ToString());
            }
        }
    }
}
