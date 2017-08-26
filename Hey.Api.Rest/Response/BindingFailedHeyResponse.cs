using System;
using System.Web.Http;
using Hey.Api.Rest.Service;
using Hey.Core.Models;

namespace Hey.Api.Rest.Response
{
    public class BindingFailedHeyResponse : IHeyResponse
    {
        public IHttpActionResult Execute(ApiController controller)
        {
            throw new NotImplementedException();
        }
    }
}