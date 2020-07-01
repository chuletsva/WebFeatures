using System;

namespace Application.Exceptions
{
    public class FailedAuthorizationException : Exception
    {
        public FailedAuthorizationException() : base("User is not authorized")
        {
        }
    }
}
