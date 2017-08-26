using System;
using Hangfire;

namespace Hey.Api.Rest.Schedules
{
    public class BackgroundScheduleType : IScheduleType
    {
        public static IScheduleType MakePrototype()
        {
            return new BackgroundScheduleType();
        }

        public IScheduleType Prototype()
        {
            return new BackgroundScheduleType();
        }

        public string Schedule(HeyRememberDeferredExecution deferredExecution)
        {
            return BackgroundJob.Enqueue(() => deferredExecution.Execute(deferredExecution.HeyRemember));
        }
    }
}