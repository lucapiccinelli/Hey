using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using Hey.Core.Models;
using NUnit.Framework;

namespace Hey.Api.Rest.Tests
{
    [TestFixture]
    class HeyControllerHttpRealTests
    {
        //[Ignore("To launch by hand, only if the web service is running")]
        [Test]
        public async Task TestPostJsonOnARealHttpCommunication()
        {
            var client = new HttpClient();
            var heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Soardi",
                Type = "Mail",
                Name = "Note",
                Id = "10343",
                When = new[] { DateTime.Now + TimeSpan.FromSeconds(60), DateTime.UtcNow },
                DomainSpecificData = "[10343, \"luca.picci@gmail.com\"]"
            };
            string id = HttpUtility.UrlEncode(heyObj.DomainSpecificData);
            

            using (var response = await client.PostAsJsonAsync("http://localhost:60401/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
        }

        //[Ignore("To launch by hand, only if the web service is running")]
        [Test]
        public async Task TestPostJsonOnARealHttpCommunicationWithNoIdMustSetToDeafult()
        {
            var client = new HttpClient();
            var heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Soardi",
                Type = "Mail",
                Name = "Note",
                When = new[] { DateTime.Now + TimeSpan.FromSeconds(60), DateTime.UtcNow },
                DomainSpecificData = "[10343, \"luca.picci@gmail.com\"]"
            };
            string id = HttpUtility.UrlEncode(heyObj.DomainSpecificData);


            using (var response = await client.PostAsJsonAsync("http://localhost:60401/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.True(response.Headers.Location.ToString().StartsWith("http://localhost:60401/api/Hey/Hey.Soardi/Mail/Note/Default"));
            }
        }

        //[Ignore("To launch by hand, only if the web service is running")]
        [Test]
        public async Task TesDeleteOnARealHttpCommunication()
        {
            var client = new HttpClient();
            using (var response = await client.DeleteAsync("http://localhost:60401/api/Hey/10343"))
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Ignore("To launch by hand, only if the web service is running")]
        [Test]
        public async Task WhenTheDomainCantBeFoundAsAnAssemblyMustReturnBadRequest()
        {
            var client = new HttpClient();
            var heyObj = new HeyRememberDto()
            {
                Domain = "Hey.Banana",
            };


            using (var response = await client.PostAsJsonAsync("http://localhost:60402/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }
    }
}
