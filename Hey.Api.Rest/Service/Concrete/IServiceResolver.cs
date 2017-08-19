using Hey.Core.Models;

namespace Hey.Api.Rest.Service.Concrete
{
    public interface IServiceResolver
    {
        IHangfireService Find(HeyRememberDto heyRemember);
    }
}