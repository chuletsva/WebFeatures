using System;

namespace WebApi.OData
{
    internal class ODataException : Exception
    {
        public ODataException(string message) : base(message)
        {
        }
    }
}
