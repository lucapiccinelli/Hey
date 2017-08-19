using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Controllers
{
    public class HeyController : ApiController
    {
        private readonly IHeyService _heyService;

        public HeyController(IHeyService heyService)
        {
            _heyService = heyService;
        }

        // GET: api/Hey
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Hey/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Hey
        [ResponseType(typeof(HeyRememberDto))]
        public IHttpActionResult Post([FromBody]HeyRememberDto heyRemember)
        {
            IHeyResponse heyResponse = _heyService.Handle(heyRemember);
            return CreatedAtRoute("DefaultApi", new {id = heyRemember.Id}, heyRemember);
        }

        // PUT: api/Hey/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hey/5
        public void Delete(int id)
        {
        }
    }
}
