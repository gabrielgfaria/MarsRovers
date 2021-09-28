using System;

namespace Services.Exceptions
{
    [Serializable]
    public class InvalidHeadingException : Exception
    {
        public InvalidHeadingException() { }

        public InvalidHeadingException(string heading)
            : base($"Invalid heading: {heading}")
        {

        }
    }
}
