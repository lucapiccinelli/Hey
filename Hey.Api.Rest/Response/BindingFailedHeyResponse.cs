using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class BindingFailedHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly ILog _log;

        public BindingFailedHeyResponse(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            string msg = $"heyRember request {_heyRemember} can't be binded to any method for an unknown reason";
            _log.Error(msg);
            return controller.ExposedBadRequest(msg);
        }
    }
}