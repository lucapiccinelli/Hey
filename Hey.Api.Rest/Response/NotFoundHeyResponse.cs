using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;

namespace Hey.Api.Rest.Response
{
    public class NotFoundHeyResponse : IHeyResponse
    {
        private readonly string _id;

        public NotFoundHeyResponse(string id)
        {
            _id = id;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedNotFound();
        }
    }
}