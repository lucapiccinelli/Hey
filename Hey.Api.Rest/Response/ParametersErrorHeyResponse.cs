using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class ParametersErrorHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;
        public int ParamNum { get; }

        public ParametersErrorHeyResponse(HeyRememberDto heyRemember, int paramNum)
        {
            _heyRemember = heyRemember;
            ParamNum = paramNum;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedBadRequest($"Error on parameters. Ensure that DomainSpecific data are in a correct json form. This are yours: {_heyRemember.DomainSpecificData}. If they are, than have a look to the parameter number {ParamNum + 1}");
        }
    }
}