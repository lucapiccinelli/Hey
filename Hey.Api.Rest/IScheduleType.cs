namespace Hey.Api.Rest
{
    public interface IScheduleType
    {
        IScheduleType Prototype();
        string Schedule(HeyRememberDeferredExecution deferredExecution);
    }
}