using System;

namespace Application.Common.Exceptions
{
    public class FailedAuthorizationException : Exception
    {
        public FailedAuthorizationException() : base("User is not authorized")
        {
        }
    }
}