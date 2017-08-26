using System;

namespace Hey.Api.Rest.Exceptions
{
    public interface IHeyExceptionHandler
    {
        void Handle(Exception ex);
    }
}