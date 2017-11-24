using System.Collections.Generic;
using Hey.Core.Models;

namespace Hey.Api.Rest.Service
{
    public interface IHeyService
    {
        IHeyResponse Create(HeyRememberDto heyRemember, bool update = false);
        List<HeyRememberResultDto> Find(string id, bool listSucceded = false);
        IHeyResponse Delete(string id);
        IHeyResponse Update(string id, HeyRememberDto heyRemember);
    }
}