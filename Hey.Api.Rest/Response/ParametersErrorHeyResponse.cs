using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class ParametersErrorHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;
        private readonly ILog _log;
        public int ParamNum { get; }

        public ParametersErrorHeyResponse(HeyRememberDto heyRemember, int paramNum)
        {
            _heyRemember = heyRemember;
            ParamNum = paramNum;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            string msg = $"Error on parameters. Ensure that DomainSpecific data are in a correct json form. This are yours: {_heyRemember.DomainSpecificData}. If they are, than have a look to the parameter number {ParamNum + 1}";
            _log.Warn(msg);
            return controller.ExposedBadRequest(msg);
        }
    }
}