using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class ErrorHeyResponse : IHeyResponse
    {
        private readonly Exception _exception;
        private readonly HeyRememberDto _heyRemember;
        private readonly ILog _log;

        public ErrorHeyResponse(Exception exception, HeyRememberDto heyRemember)
        {
            _exception = exception;
            _heyRemember = heyRemember;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            _log.Error($"{_heyRemember}: error processing this request");
            return controller.ExposedInternalServerError(_exception);
        }
    }
}