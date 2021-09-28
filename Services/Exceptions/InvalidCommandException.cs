using System;

namespace Services.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }

        public InvalidCommandException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }
}
