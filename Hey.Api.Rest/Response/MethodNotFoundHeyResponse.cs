using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class MethodNotFoundHeyResponse : IHeyResponse
    {
        private readonly HeyRememberDto _heyRemember;

        public MethodNotFoundHeyResponse(HeyRememberDto heyRemember)
        {
            _heyRemember = heyRemember;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedBadRequest($"{_heyRemember.Id} not found in {_heyRemember.Domain}\\{_heyRemember.Type}");
        }
    }
}