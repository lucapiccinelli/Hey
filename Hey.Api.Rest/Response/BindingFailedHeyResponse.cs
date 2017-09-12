using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class BindingFailedHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;

        public BindingFailedHeyResponse(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedBadRequest($"heyRember request {_heyRemember} can't be binded to any method for an unknown reason");
        }
    }
}