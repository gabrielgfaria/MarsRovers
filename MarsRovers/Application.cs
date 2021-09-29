using Models;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarsRovers
{
    public class Application : IApplication
    {
        private readonly IPlateuService _plateauService;
        private readonly IRoverService _roverService;

        public Application(IPlateuService plateauService,
            IRoverService roverService)
        {
            _plateauService = plateauService;
            _roverService = roverService;
        }

        public void Run()
        {
            var configurationAndCommands = File.ReadAllLines("..\\..\\..\\..\\InputFile.txt");

            ConfigurePlateauAndSendCommands(configurationAndCommands.ToList());
            PrintOutRoversPositions();
            Console.ReadLine();
        }

        private void PrintOutRoversPositions()
        {
            var roversPositionsAndHeading = _roverService.GetRoversPositionsAndHeading();
            foreach(var roverPositionAndHeading in roversPositionsAndHeading)
            {
                Console.WriteLine(roverPositionAndHeading);
            }
        }

        private void ConfigurePlateauAndSendCommands(List<string> configurationAndCommands)
        {
            var plateauConfiguration = GetPlateauConfiguration(configurationAndCommands);
            configurationAndCommands.RemoveAt(0);
            
            _plateauService.EstablishBoundaries(plateauConfiguration);
            SendCommandsToRover(configurationAndCommands);
        }

        private void SendCommandsToRover(List<string> commands)
        {
            var commandsFinished = commands.Count == 0;
            while(!commandsFinished)
            {
                var roverInitialPosition = GetRoverInitialPosition(commands.First());
                var roverInitialHeading = GetRoverInitialHeading(commands.First());
                _roverService.InitiateRover(roverInitialPosition, roverInitialHeading);
                commands.RemoveAt(0);
                _roverService.SendCommands(commands.First());
                commands.RemoveAt(0);

                commandsFinished = commands.Count == 0;
            }
        }

        private static string GetRoverInitialHeading(string command)
        {
            var rawPositionAndHeading = command.Split(' ');
            var heading = rawPositionAndHeading.Last();

            return heading;
        }

        private static Position GetRoverInitialPosition(string command)
        {
            var rawPositionAndHeading = command.Split(' ');
            var position = new Position();
            try
            {
                position.X = int.Parse(rawPositionAndHeading.First());
                position.Y = int.Parse(rawPositionAndHeading.ElementAt(1));
            }
            catch (Exception)
            {
                throw new Exception("Invalid input format: Rover initial position was not present in the expected format");
            }

            return position;
        }

        private static (int x, int y) GetPlateauConfiguration(List<string> configurationAndCommands)
        {
            var rawConfiguration = configurationAndCommands.First().Split(' ');
            (int x, int y) plateauBoundaries;

            try
            {
                plateauBoundaries.x = int.Parse(rawConfiguration.First());
                plateauBoundaries.y = int.Parse(rawConfiguration.Last());
            }
            catch (Exception)
            {
                throw new Exception("Invalid input format: Boundaries were not present in the expected format");
            }

            return plateauBoundaries;
        }
    }
}
