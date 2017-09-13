using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class UpdatedHttpReturn : IHttpReturnType
    {
        public IHttpActionResult Return(string heyId, HeyRememberDto heyRemember, HeyController controller)
        {
            return controller.ExposedOk();
        }
    }
}