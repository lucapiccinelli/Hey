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
using log4net;

namespace Hey.Api.Rest.Response
{
    public class OkHeyResponse : IHeyResponse
    {
        private readonly IMethodBinder _methodBinder;
        private readonly IScheduleType _scheduleType;
        private ILog _log;


        public OkHeyResponse(IMethodBinder methodBinder, IScheduleType scheduleType)
        {
            _methodBinder = methodBinder;
            _scheduleType = scheduleType;

            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            HeyRememberDeferredExecution deferredExecution = _methodBinder.CreateDeferredExecution();
            HeyRememberDto heyRemember = deferredExecution.HeyRemember;
            try
            {

                string jobId = _scheduleType.Schedule(deferredExecution);
                string heyId = $"{heyRemember.Domain}/{(heyRemember.Type != string.Empty ? heyRemember.Type + "/" : string.Empty)}{heyRemember.Name}/{jobId}";

                _log.Info($"{heyRemember} scheduled on {heyId}");

                return controller.ExposedCreatedAtRoute("DefaultApi", new { id = heyId }, heyRemember);
            }
            catch (Exception ex)
            {
                _log.Error($"{heyRemember}: Error while trying to schedule job", ex);
                return controller.ExposedInternalServerError(ex);
            }
        }
    }
}