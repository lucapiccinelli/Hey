using System;
using System.Web.Http;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class BindingFailedHeyResponse : IHeyResponse
    {
        public IHttpActionResult Execute(ApiController controller, HeyRememberDto heyRemember)
        {
            throw new NotImplementedException();
        }

        public IHeyResponse Prototype(IMethodBinder methodBinder)
        {
            return new BindingFailedHeyResponse();
        }

        public static BindingFailedHeyResponse MakePrototype()
        {
            return new BindingFailedHeyResponse();
        }
    }
}