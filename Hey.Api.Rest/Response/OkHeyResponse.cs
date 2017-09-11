using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Hangfire;
using Hangfire.Server;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Exceptions;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class OkHeyResponse : IHeyResponse
    {
        private readonly IMethodBinder _methodBinder;
        private readonly IScheduleType _scheduleType;
        public string HeyId { get; private set; }


        public OkHeyResponse(IMethodBinder methodBinder, IScheduleType scheduleType)
        {
            _methodBinder = methodBinder;
            _scheduleType = scheduleType;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            HeyRememberDeferredExecution deferredExecution = _methodBinder.CreateDeferredExecution();
            HeyRememberDto heyRemember = deferredExecution.HeyRemember;

            string jobId = _scheduleType.Schedule(deferredExecution);
            HeyId = $"{heyRemember.Domain}/{(heyRemember.Type != string.Empty ? heyRemember.Type + "/" : string.Empty)}{heyRemember.Id}/{jobId}";
            return controller.ExposedCreatedAtRoute("DefaultApi", new { id = HeyId }, heyRemember);
        }
    }
}