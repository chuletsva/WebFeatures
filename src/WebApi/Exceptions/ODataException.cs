using System;

namespace WebApi.Exceptions
{
    internal class ODataException : Exception
    {
        public ODataException(string message) : base(message)
        {
        }
    }
}
