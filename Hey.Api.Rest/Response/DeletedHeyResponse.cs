using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Api.Rest.Service;
using log4net;

namespace Hey.Api.Rest.Response
{
    public class DeletedHeyResponse : IHeyResponse
    {
        private readonly string _id;
        private readonly ILog _log;

        public DeletedHeyResponse(string id)
        {
            _id = id;
            _log = LogManager.GetLogger(GetType());
        }

        public IHttpActionResult Execute(HeyController controller)
        {
            string msg = $"The resources {_id} have been deleted on user request";
            _log.Info(msg);
            return controller.ExposedOk();
        }
    }
}