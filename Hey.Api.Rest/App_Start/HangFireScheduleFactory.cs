using Hey.Api.Rest.Schedules;
using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public class HangFireScheduleFactory : IScheduleTypeFactory
    {
        public IScheduleType MakeAPrototype(HeyRememberDto heyRemember)
        {
            return DelayedScheduleType.MakePrototype();
        }
    }
}