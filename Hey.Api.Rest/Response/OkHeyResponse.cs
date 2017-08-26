using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Hangfire;
using Hangfire.Server;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class OkHeyResponse : IHeyResponse
    {
        private readonly bool _isAPrototype;
        private readonly IMethodBinder _methodBinder;
        private readonly IScheduleType _scheduleType;

        private OkHeyResponse(bool isAPrototype)
        {
            _isAPrototype = isAPrototype;
        }

        public OkHeyResponse(IMethodBinder methodBinder, IScheduleType scheduleType)
            : this(false)
        {
            _methodBinder = methodBinder;
            _scheduleType = scheduleType;
        }

        public IHttpActionResult Execute(ApiController controller)
        {
            if (_isAPrototype)
            {
                throw new ThisObjectIsAPrototypeException(this);
            }

            HeyRememberDeferredExecution deferredExecution = _methodBinder.CreateDeferredExecution();
            HeyRememberDto heyRemember = deferredExecution.HeyRemember;

            string jobId = _scheduleType.Schedule(deferredExecution);
            string heyId = 
                $"{heyRemember.Domain}/{(heyRemember.Type != string.Empty ? heyRemember.Type + "/" : string.Empty)}{heyRemember.Id}/{jobId}";
            return new CreatedAtRouteNegotiatedContentResult<HeyRememberDto>("DefaultApi", new Dictionary<string, object> { { "id", heyId } }, deferredExecution.HeyRemember, controller);
        }
    }
}