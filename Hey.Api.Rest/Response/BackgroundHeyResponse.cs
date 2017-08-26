using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Hangfire;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class BackgroundHeyResponse : IHeyResponse
    {
        private readonly bool _isAPrototype;
        private readonly IMethodBinder _methodBinder;

        private BackgroundHeyResponse(bool isAPrototype)
        {
            _isAPrototype = isAPrototype;
        }

        public BackgroundHeyResponse(IMethodBinder methodBinder)
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

            string jobId = BackgroundJob.Enqueue(() => deferredExecution.Execute(deferredExecution.HeyRemember));
            return new CreatedAtRouteNegotiatedContentResult<HeyRememberDto>("DefaultApi", new Dictionary<string, object> {{"id", jobId}}, heyRemember, controller);
        }

        public IHeyResponse Prototype(IMethodBinder methodBinder)
        {
            return new BackgroundHeyResponse(methodBinder);
        }
        public static BackgroundHeyResponse MakePrototype()
        {
            return new BackgroundHeyResponse(true);
        }
    }
}