using Models;
using Services.Exceptions;

namespace Services
{
    public class PlateuService : IPlateuService
    {
        public void EstablishBoundaries((int x, int y) boundaries)
        {
            ValidateBoundaries(boundaries);
            Plateau.SetBoundaries(boundaries);
        }

        private static void ValidateBoundaries((int x, int y) boundaries)
        {
            if (boundaries.x < 0 || boundaries.y < 0)
            {
                throw new InvalidBoundariesException(boundaries.ToString());
            }
        }
    }
}
