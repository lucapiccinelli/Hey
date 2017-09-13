using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using Hangfire.Storage;
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

        // GET: api/Hey/Id
        public IEnumerable<HeyRememberResultDto> Get(string id)
        {
            return _heyService.Find(id);
        }

        // POST: api/Hey
        [ResponseType(typeof(HeyRememberDto))]
        public IHttpActionResult Post([FromBody]HeyRememberDto heyRemember)
        {
            IHeyResponse heyResponse = _heyService.Create(heyRemember);
            return heyResponse.Execute(this);
        }

        // PUT: api/Hey/5
        public IHttpActionResult Put(string id, [FromBody]HeyRememberDto heyRemember)
        {
            heyRemember.Id = id;
            IHeyResponse heyResponse = _heyService.Update(id, heyRemember);
            return heyResponse.Execute(this);
        }

        // DELETE: api/Hey/5
        public IHttpActionResult Delete(string id)
        {
            IHeyResponse heyResponse = _heyService.Delete(id);
            return heyResponse.Execute(this);
        }

        [NonAction]
        public IHttpActionResult ExposedCreatedAtRoute<T>(string routeName, object routeValues, T content)
        {
            return CreatedAtRoute(routeName, routeValues, content);
        }

        [NonAction]
        public IHttpActionResult ExposedBadRequest(string message)
        {
            return BadRequest(message);
        }

        [NonAction]
        public IHttpActionResult ExposedInternalServerError(Exception ex)
        {
            return InternalServerError(ex);
        }

        [NonAction]
        public IHttpActionResult ExposedNotFound()
        {
            return NotFound();
        }

        [NonAction]
        public IHttpActionResult ExposedOk()
        {
            return Ok();
        }
    }
}
