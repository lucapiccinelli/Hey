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

        // GET: api/Hey
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Hey/Id
        [ResponseType(typeof(List<RecurringJobDto>))]
        public IEnumerable<RecurringJobDto> Get(string id)
        {
            List<RecurringJobDto> recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            List<RecurringJobDto> filteredRecurringJobs = recurringJobs.FindAll(dto => dto.Id.StartsWith(id));

            return filteredRecurringJobs;
        }

        // POST: api/Hey
        [ResponseType(typeof(HeyRememberDto))]
        public IHttpActionResult Post([FromBody]HeyRememberDto heyRemember)
        {
            IHeyResponse heyResponse = _heyService.Create(heyRemember);
            return heyResponse.Execute(this);
        }

        // PUT: api/Hey/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hey/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult ExposedCreatedAtRoute<T>(string routeName, object routeValues, T content)
        {
            return CreatedAtRoute(routeName, routeValues, content);
        }
    }
}
