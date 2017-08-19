using Hey.Api.Rest.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyService
    {
        IHeyResponse Handle(HeyRememberDto heyRemember);
    }
}