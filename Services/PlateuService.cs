using Models;
using Services.Exceptions;

namespace Services
{
    public class PlateuService : IPlateuService
    {
        public void EstablishBoundaries(Position boundaries)
        {
            ValidateBoundaries(boundaries);
            Plateau.SetBoundaries(boundaries);
        }

        private static void ValidateBoundaries(Position boundaries)
        {
            if (boundaries.X < 0 || boundaries.Y < 0)
            {
                throw new InvalidBoundariesException(boundaries.ToString());
            }
        }
    }
}
