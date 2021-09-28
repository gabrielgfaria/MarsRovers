using Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public static class RoverMap
    {
        public static List<Rover> Rovers = new List<Rover>();
        public static Dictionary<Position, Rover> RoversLocation = new Dictionary<Position, Rover>();

        public static void Add(Rover rover)
        {
            rover.Id = $"Rover_{Rovers.Count + 1}";
            Rovers.Add(rover);
            RoversLocation.Add(rover.Position, rover);
        }
    }
}
