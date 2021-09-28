using Models;

namespace Services
{
    public interface IRoverService
    {
        void InitiateRover(Position initialPosition, string heading);
        void SendCommands(string requiredCommands);
    }
}