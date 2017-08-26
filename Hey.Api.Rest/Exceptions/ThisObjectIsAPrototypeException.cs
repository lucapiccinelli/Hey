using System;
using Hey.Api.Rest.Response;

namespace Hey.Api.Rest.Exceptions
{
    public class ThisObjectIsAPrototypeException : Exception
    {
        private readonly object _heyResponse;

        public ThisObjectIsAPrototypeException(object heyResponse)
            : base($"Prototype {heyResponse.GetType()} can't be called for execution")
        {
            _heyResponse = heyResponse;
        }
    }
}