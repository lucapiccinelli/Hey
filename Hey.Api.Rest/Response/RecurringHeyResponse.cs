using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Hangfire;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class RecurringHeyResponse : IHeyResponse
    {
        private readonly bool _isAPrototype;
        private readonly IMethodBinder _methodBinder;

        private RecurringHeyResponse(bool isAPrototype)
        {
            _isAPrototype = isAPrototype;
        }

        public RecurringHeyResponse(IMethodBinder methodBinder)
            : this(false)
        {
            _methodBinder = methodBinder;
        }

        public IHttpActionResult Execute(ApiController controller, HeyRememberDto heyRemember)
        {
            if (_isAPrototype)
            {
                throw new ThisObjectIsAPrototypeException(this);
            }

            HeyRememberDeferredExecution deferredExecution = _methodBinder.CreateDeferredExecution();

            string jobId = deferredExecution.HeyRemember.Id;
            RecurringJob.AddOrUpdate(jobId, () => deferredExecution.Execute(deferredExecution.HeyRemember), Cron.Minutely);
            return new CreatedAtRouteNegotiatedContentResult<HeyRememberDto>("DefaultApi", new Dictionary<string, object> { { "id", jobId } }, heyRemember, controller);
        }

        public IHeyResponse Prototype(IMethodBinder methodBinder)
        {
            return new RecurringHeyResponse(methodBinder);
        }
        public static RecurringHeyResponse MakePrototype()
        {
            return new RecurringHeyResponse(true);
        }
    }
}