using System.Collections.Generic;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public interface IJobRepository
    {
        IScheduleType MakeASchedulePrototype(HeyRememberDto heyRemember);
        List<HeyRememberResultDto> GetJobs(string id);
    }
}