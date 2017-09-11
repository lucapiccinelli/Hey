using System.Web.Http;
using Hey.Api.Rest.Controllers;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyResponse
    {
        IHttpActionResult Execute(HeyController controller);
    }
}