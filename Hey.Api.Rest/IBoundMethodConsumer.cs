using System.Reflection;

namespace Hey.Api.Rest
{
    public interface IBoundMethodConsumer
    {
        MethodExecutionResultEnum Use(MethodInfo method, object obj, object[] myParams);
    }
}