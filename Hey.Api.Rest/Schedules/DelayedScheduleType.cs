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
            return BackgroundJob.Schedule(
                () => deferredExecution.Execute(heyRemember),
                new DateTimeOffset(heyRemember.When[0]));
        }

        public static IScheduleType MakePrototype()
        {
            return new DelayedScheduleType();
        }
    }
}