using System.Web;
using Hangfire;

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
            string id = HttpUtility.UrlEncode(deferredExecution.HeyRemember.DomainSpecificData.Normalize());
            RecurringJob.AddOrUpdate(id, () => deferredExecution.Execute(deferredExecution.HeyRemember), Cron.Minutely);
            return id;
        }
    }
}