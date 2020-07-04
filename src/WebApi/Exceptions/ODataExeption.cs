using System;

namespace WebApi.Exceptions
{
    class ODataExeption : Exception
    {
        public ODataExeption(string message) : base(message)
        {
        }
    }
}
