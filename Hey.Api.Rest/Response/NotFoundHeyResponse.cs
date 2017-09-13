using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class NotFoundHeyResponse : IHeyResponse
    {
        private readonly string _id;
        private readonly ILog _log;

        public NotFoundHeyResponse(string id)
        {
            _id = id;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            string msg = $"The requestd resource {_id} doesn't exist";
            _log.Warn(msg);
            return controller.ExposedNotFound();
        }
    }
}