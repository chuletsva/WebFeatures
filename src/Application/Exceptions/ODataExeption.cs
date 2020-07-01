using System;

namespace Application.Exceptions
{
    public class ODataExeption : Exception
    {
        public ODataExeption(string message) : base(message)
        {
        }
    }
}
