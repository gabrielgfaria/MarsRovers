using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IRoverService
    {
        void InitiateRover(Position initialPosition, string heading);
        void SendCommands(string requiredCommands);
        List<string> GetRoversPositionsAndHeading(); 
    }
}