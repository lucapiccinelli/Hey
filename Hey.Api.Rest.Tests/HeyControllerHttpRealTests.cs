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
                When = new []{DateTime.Now + TimeSpan.FromSeconds(60), DateTime.UtcNow},
                DomainSpecificData = "[10343, \"luca.picci@gmail.com\"]"
            };
            string id = HttpUtility.UrlEncode(heyObj.DomainSpecificData);
            

            using (var response = await client.PostAsJsonAsync("http://localhost:60402/api/Hey", heyObj))
            {
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            }
        }

        //[Ignore("To launch by hand, only if the web service is running")]
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
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
