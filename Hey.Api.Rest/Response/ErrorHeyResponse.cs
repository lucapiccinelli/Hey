using System;
using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;

namespace Hey.Api.Rest.Response
{
    public class ErrorHeyResponse : IHeyResponse
    {
        private readonly Exception _exception;

        public ErrorHeyResponse(Exception exception)
        {
            _exception = exception;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedBadRequest(_exception.Message);
        }
    }
}