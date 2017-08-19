using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyService
    {
        IHeyResponse Handle(HeyRememberDto heyRemember);
    }
}