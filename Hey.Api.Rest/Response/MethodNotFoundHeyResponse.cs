using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class MethodNotFoundHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly ILog _log;

        public MethodNotFoundHeyResponse(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            _log.Warn($"{_heyRemember}: method not found");
            return controller.ExposedBadRequest($"{_heyRemember.Name} not found in {_heyRemember.Domain}\\{_heyRemember.Type}");
        }
    }
}