using System;

namespace Services.Exceptions
{
    public class InvalidBoundariesException : Exception
    {
        public InvalidBoundariesException() { }

        public InvalidBoundariesException(string boundaries)
            : base($"Invalid Plateau boundaries: {boundaries}")
        {
        }
    }
}
