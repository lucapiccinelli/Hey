using System.Reflection;

namespace Hey.Api.Rest
{
    public class HeyInvokeMethod : IBoundMethodConsumer
    {
        public MethodExecutionResultEnum Use(MethodInfo method, object obj, object[] myParams)
        {
            method.Invoke(obj, myParams);
            return MethodExecutionResultEnum.Ok;
        }
    }
}