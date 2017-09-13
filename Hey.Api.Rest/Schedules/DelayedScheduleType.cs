using System;
using Hangfire;
using Hey.Core.Models;

namespace Hey.Api.Rest.Schedules
{
    public class DelayedScheduleType : IScheduleType
    {
        public IScheduleType Prototype()
        {
            return new DelayedScheduleType();
        }

        public string Schedule(HeyRememberDeferredExecution deferredExecution)
        {
            HeyRememberDto heyRemember = deferredExecution.HeyRemember;
            return heyRemember.When[0] <= DateTime.Now
                ? BackgroundJob.Enqueue(() => deferredExecution.Execute(heyRemember))
                : BackgroundJob.Schedule(() => deferredExecution.Execute(heyRemember), new DateTimeOffset(heyRemember.When[0]));
        }

        public static IScheduleType MakePrototype()
        {
            return new DelayedScheduleType();
        }
    }
}