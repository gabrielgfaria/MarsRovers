using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RoverService : IRoverService
    {
        private static readonly char[] _POSSIBLEHEADINGS = { 'N', 'E', 'S', 'W' };
        private static readonly char[] _POSSIBLECOMMANDS = { 'L', 'R', 'M' };

        public void InitiateRover(Position initialPosition, string heading)
        {
            heading = heading.Trim().ToUpper();
            ValidateHeading(heading);
            ValidatePosition(initialPosition);
            var rover = new Rover
            {
                Heading = heading.Single(),
                Position = initialPosition
            };
            CheckForCollision(rover);

            RoverMap.Add(rover);
        }

        public void SendCommands(string requiredCommands)
        {
            var rover = RoverMap.Rovers.Last();
            requiredCommands = requiredCommands.Replace(" ", "").ToUpper();

            ValidateCommands(requiredCommands);
            CheckForCollision(rover, requiredCommands);
            CheckForOutOfBounds(rover, requiredCommands);

            foreach (var command in requiredCommands)
            {
                ExecuteCommand(rover, command);
            }
        }

        public List<string> GetRoversPositionsAndHeading()
        {
            var roversPositionsAndHeadings = new List<string>();
            foreach(var roverPositionAndHeading in RoverMap.RoversLocation)
            {
                roversPositionsAndHeadings.Add($"{roverPositionAndHeading.Key.X} {roverPositionAndHeading.Key.Y} {roverPositionAndHeading.Value.Heading}");
            }

            return roversPositionsAndHeadings;
        }

        private static void TurnRight(Rover rover)
        {
            switch (rover.Heading)
            {
                case 'N':
                    rover.Heading = 'E';
                    break;
                case 'E':
                    rover.Heading = 'S';
                    break;
                case 'S':
                    rover.Heading = 'W';
                    break;
                case 'W':
                    rover.Heading = 'N';
                    break;
            }
        }

        private static void TurnLeft(Rover rover)
        {
            switch (rover.Heading)
            {
                case 'N':
                    rover.Heading = 'W';
                    break;
                case 'E':
                    rover.Heading = 'N';
                    break;
                case 'S':
                    rover.Heading = 'E';
                    break;
                case 'W':
                    rover.Heading = 'S';
                    break;
            }
        }

        private static void Move(Rover rover, bool isSimulation = false)
        {
            if(!isSimulation)
                RoverMap.RoversLocation.Remove(rover.Position);

            switch (rover.Heading)
            {
                case 'N':
                    rover.Position.Y++;
                    break;
                case 'E':
                    rover.Position.X++;
                    break;
                case 'S':
                    rover.Position.Y--;
                    break;
                case 'W':
                    rover.Position.X--;
                    break;
            }

            if (!isSimulation)
                RoverMap.RoversLocation.Add(rover.Position, rover);
        }

        private static void CheckForCollision(Rover rover, string requiredCommands = "")
        {
            var simulatedRover = new Rover()
            {
                Id = rover.Id,
                Heading = rover.Heading,
                Position = new Position() { X = rover.Position.X, Y = rover.Position.Y }
            };

            if (string.IsNullOrEmpty(requiredCommands))
            {
                if (RoverMap.RoversLocation.ContainsKey(simulatedRover.Position) &&
                    RoverMap.RoversLocation[simulatedRover.Position].Id != simulatedRover.Id)
                {
                    var collisionRoverId = RoverMap.RoversLocation[simulatedRover.Position].Id;
                    throw new InvalidCommandException($"Unable to deploy rover to this position. It would collide with {collisionRoverId}.");
                }
            }

            foreach (var command in requiredCommands)
            {
                ExecuteCommand(simulatedRover, command, true);
                if (RoverMap.RoversLocation.ContainsKey(simulatedRover.Position) &&
                    RoverMap.RoversLocation[simulatedRover.Position].Id != simulatedRover.Id)
                {
                    var collisionRoverId = RoverMap.RoversLocation[simulatedRover.Position].Id;
                    throw new InvalidCommandException($"These commands would generate a collision between {rover.Id} and {collisionRoverId}. Commands: {requiredCommands}");
                }
            }
        }

        private static void CheckForOutOfBounds(Rover rover, string requiredCommands)
        {
            var simulatedRover = new Rover()
            {
                Id = rover.Id,
                Heading = rover.Heading,
                Position = new Position() { X = rover.Position.X, Y = rover.Position.Y }
            };

            foreach (var command in requiredCommands)
            {
                ExecuteCommand(simulatedRover, command, true);
                if (simulatedRover.Position.X > Plateau.Boundaries.X || simulatedRover.Position.Y > Plateau.Boundaries.Y ||
                    simulatedRover.Position.X < 0 || simulatedRover.Position.Y < 0)
                {
                    throw new InvalidCommandException($"These commands would send the rover out of bounds: {requiredCommands}");
                }
            }
        }

        private static void ExecuteCommand(Rover rover, char command, bool isSimulation = false)
        {
            switch (command)
            {
                case 'L':
                    TurnLeft(rover);
                    break;
                case 'R':
                    TurnRight(rover);
                    break;
                case 'M':
                    Move(rover, isSimulation);
                    break;
                default:
                    break;
            }
        }

        private static void ValidateCommands(string commands)
        {
            var invalidCommands = commands.Except(_POSSIBLECOMMANDS);
            if (invalidCommands.Any())
            {
                throw new InvalidCommandException($"Invalid command detected: {invalidCommands}");
            }
        }

        private static void ValidateHeading(string heading)
        {
            if (!_POSSIBLEHEADINGS.Select(x => x.ToString()).Contains(heading))
            {
                throw new InvalidHeadingException(heading);
            }
        }

        private static void ValidatePosition(Position position)
        {
            if (position.X > Plateau.Boundaries.X || position.Y > Plateau.Boundaries.Y ||
                position.X < 0 || position.Y < 0)
            {
                throw new InvalidInitialPositionException(position.ToString());
            }
        }
    }
}
