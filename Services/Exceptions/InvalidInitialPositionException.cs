using System;

namespace Services.Exceptions
{
    public class InvalidInitialPositionException : Exception
    {
        public InvalidInitialPositionException() { }

        public InvalidInitialPositionException(string position)
            : base($"Invalid initial position: {position}")
        {
        }
    }
}
