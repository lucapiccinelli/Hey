using System;

namespace Hey.Api.Rest.Exceptions
{
    public class DeferredExecutionException : Exception
    {
        public DeferredExecutionException(string msg)
            :base(msg)
        {
            
        }
    }
}