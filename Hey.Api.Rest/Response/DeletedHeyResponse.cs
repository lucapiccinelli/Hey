using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;

namespace Hey.Api.Rest.Response
{
    public class DeletedHeyResponse : IHeyResponse
    {
        private readonly string _id;

        public DeletedHeyResponse(string id)
        {
            _id = id;
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            return controller.ExposedOk();
        }
    }
}