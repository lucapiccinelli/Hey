using Hey.Core.Models;

namespace Hey.Api.Rest
{
    public interface IScheduleTypeFactory
    {
        IScheduleType MakeAPrototype(HeyRememberDto heyRemember);
    }
}