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
            throw new System.NotImplementedException();
        }
    }
}