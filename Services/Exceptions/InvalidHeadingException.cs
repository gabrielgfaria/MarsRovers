using System;

namespace Services.Exceptions
{
    public class InvalidHeadingException : Exception
    {
        public InvalidHeadingException() { }

        public InvalidHeadingException(string heading)
            : base($"Invalid heading: {heading}")
        {

        }
    }
}
