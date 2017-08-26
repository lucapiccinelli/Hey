namespace Hey.Api.Rest
{
    public interface IMethodBinder
    {
        MethodExecutionResultEnum Invoke();
        MethodExecutionResultEnum Invoke(IBoundMethodConsumer consumer);
        string Name { get; }
        HeyRememberDeferredExecution CreateDeferredExecution();
    }
}