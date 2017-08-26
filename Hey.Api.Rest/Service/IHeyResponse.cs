using System.Web.Http;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyResponse
    {
        IHttpActionResult Execute(ApiController controller, HeyRememberDto heyRemember);
        IHeyResponse Prototype(IMethodBinder methodBinder);
    }
}