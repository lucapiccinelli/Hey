using System.Reflection;

namespace Hey.Api.Rest
{
    public class BinderCanCallTheMethod : IBoundMethodConsumer
    {
        public MethodExecutionResultEnum ExecutionResultEnum { get; }

        public BinderCanCallTheMethod(IMethodBinder methodBinder)
        {
            Can = false;
            ExecutionResultEnum = methodBinder.Invoke(this);
            Can = Can && ExecutionResultEnum == MethodExecutionResultEnum.Ok;
        }

        public MethodExecutionResultEnum Use(MethodInfo method, object obj, object[] myParams)
        {
            ObjOk = obj.GetType() == method.DeclaringType;
            ParameterInfo[] parametersInfo = method.GetParameters();

            ParametersOkNum = 0;
            int length = myParams?.Length ?? 0;

            if (parametersInfo.Length == length)
            {
                for (int i = 0; i < parametersInfo.Length; i++)
                {
                    ParameterInfo paramInfo = parametersInfo[i];
                    if (paramInfo.ParameterType != myParams[i].GetType())
                    {
                        break;
                    }
                    ParametersOkNum++;
                }
            }
            ParametersOk = ParametersOkNum == length && parametersInfo.Length == length;

            Can = ParametersOk && ObjOk;

            return MethodExecutionResultEnum.Ok;
        }

        public bool Can { get; private set; }

        public bool ParametersOk { get; private set; }

        public int ParametersOkNum { get; private set; }

        public bool ObjOk { get; private set; }
    }
}