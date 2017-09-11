using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class ParametersErrorHeyResponse : IHeyResponse
    {
        public int ParamNum { get; }

        public ParametersErrorHeyResponse(int paramNum)
        {
            ParamNum = paramNum;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            throw new NotImplementedException();
        }
    }
}