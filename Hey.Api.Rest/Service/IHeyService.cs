using System.Collections.Generic;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyService
    {
        IHeyResponse Create(HeyRememberDto heyRemember);
        List<HeyRememberResultDto> Find(string id);
    }
}