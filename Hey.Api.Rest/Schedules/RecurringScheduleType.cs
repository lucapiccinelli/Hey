using System.Web;
using Hangfire;
using Hey.Core.Models;

namespace Hey.Api.Rest.Schedules
{
    public class RecurringScheduleType : IScheduleType
    {
        public static IScheduleType MakePrototype()
        {
            return new RecurringScheduleType();
        }

        public IScheduleType Prototype()
        {
            return new RecurringScheduleType();
        }

        public string Schedule(HeyRememberDeferredExecution deferredExecution)
        {
            HeyRememberDto heyRemember = deferredExecution.HeyRemember;
            string id = $"{heyRemember.Type}/{heyRemember.When[0]}/{heyRemember.CronExpression}".GetHashCode().ToString();
            RecurringJob.AddOrUpdate(id, () => deferredExecution.Execute(deferredExecution.HeyRemember), heyRemember.CronExpression);
            return id;
        }
    }
}