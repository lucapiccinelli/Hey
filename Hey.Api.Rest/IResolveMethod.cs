using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public interface IResolveMethod
    {
        IMethodBinder Find(HeyRememberDto heyRemember);
    }
}