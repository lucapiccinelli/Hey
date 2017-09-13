using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public interface IHttpReturnType
    {
        IHttpActionResult Return(string heyId, HeyRememberDto heyRemember, HeyController controller);
    }
}