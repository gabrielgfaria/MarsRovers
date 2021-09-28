using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using Services.Exceptions;
using Shouldly;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class RoverServiceTests
    {
        private readonly IRoverService target;

        public RoverServiceTests()
        {
            Plateau.SetBoundaries(boundaries: (5, 5));
            target = new RoverService();
        }

        [TestMethod]
        [DataRow(0, 0, "W")]
        [DataRow(1, 2, "N")]
        [DataRow(3, 3, "S")]
        [DataRow(5, 0, "E")]
        public void InitiateRoverWithValidPositionAndHeadingShouldAddRoverToMap(int expectedXCoordinate,
            int expectedYCoordinate,
            string heading)
        {
            // Arrange
            var position = new Position { X = expectedXCoordinate, Y = expectedYCoordinate };

            // Act
            target.InitiateRover(position, heading);

            // Assert
            RoverMap.Rovers.Count.ShouldBe(1);
        }

        [TestMethod]
        [DataRow(-1, -1, "W")]
        [DataRow(7, 2, "N")]
        [DataRow(5, -1, "S")]
        public void InitiateRoverWithOutOfBoundsPositionAndValidHeadingShouldThrow(int expectedXCoordinate,
            int expectedYCoordinate,
            string heading)
        {
            // Arrange
            var position = new Position { X = expectedXCoordinate, Y = expectedYCoordinate };

            // Assert
            Should.Throw<InvalidInitialPositionException>(() => target.InitiateRover(position, heading));
        }

        [TestMethod]
        [DataRow(3, 2, "West")]
        [DataRow(4, 1, "L")]
        [DataRow(0, 0, "R")]
        [DataRow(5, 5, "test")]
        [DataRow(1, 1, "1")]
        public void InitiateRoverWithValidPositionAndInvalidHeadingShouldThrow(int expectedXCoordinate,
            int expectedYCoordinate,
            string heading)
        {
            // Arrange
            var position = new Position { X = expectedXCoordinate, Y = expectedYCoordinate };

            // Assert
            Should.Throw<InvalidHeadingException>(() => target.InitiateRover(position, heading));
        }

        [TestMethod]
        [DataRow(0, 0, 'W', 0, 0, "E")]
        [DataRow(1, 0, 'W', 1, 0, "S")]
        [DataRow(3, 4, 'W', 3, 4, "W")]
        [DataRow(0, 1, 'W', 0, 1, "N")]
        public void InitiateRoverToASpotWhereAnotherResidesShouldThrow(int expectedXCoordinateRover1,
            int expectedYCoordinateRover1,
            char headingRover1,
            int expectedXCoordinateRover2,
            int expectedYCoordinateRover2,
            string headingRover2)
        {
            // Arrange
            AddRoverToMap(expectedXCoordinateRover1, expectedYCoordinateRover1, headingRover1);
            var position = new Position { X = expectedXCoordinateRover2, Y = expectedYCoordinateRover2 };

            // Assert
            Should.Throw<InvalidCommandException>(() => target.InitiateRover(position, headingRover2));
        }

        [TestMethod]
        [DataRow(1, 2, 'N', "LMLMLMLMM", 1, 3, 'N')]
        [DataRow(3, 3, 'E', "MMRMMRMRRM", 5, 1, 'E')]
        [DataRow(0, 0, 'N', "MMMMM", 0, 5, 'N')]
        [DataRow(0, 0, 'N', "RMLMRMLMRM LMrMLMrMlM", 5, 5, 'N')]
        [DataRow(5, 5, 'S', "M", 5, 4, 'S')]
        [DataRow(3, 2, 'W', "LLLLM", 2, 2, 'W')]
        [DataRow(3, 2, 'W', " LL llL ", 3, 2, 'S')]
        public void SendValidCommandsToRoverShouldChangeItsProperties(int initialXCoordinate,
            int initialYCoordinate,
            char initialHeading,
            string commands,
            int expectedXCoordinate,
            int expectedYCoordinate,
            char expectedHeading)
        {
            // Arrange
            AddRoverToMap(initialXCoordinate, initialYCoordinate, initialHeading);

            // Act
            target.SendCommands(commands);

            // Assert
            RoverMap.RoversLocation.ContainsKey(new Position() { X = expectedXCoordinate, Y = expectedYCoordinate }).ShouldBeTrue();
            RoverMap.RoversLocation[new Position() { X = expectedXCoordinate, Y = expectedYCoordinate }].Heading.ShouldBe(expectedHeading);
        }

        [TestMethod]
        [DataRow(1, 2, 'N', 0, 0, 'N', "RMLMMRMMRM")]
        [DataRow(0, 2, 'N', 0, 0, 'E', "LmM")]
        [DataRow(5, 5, 'N', 0, 0, 'N', "RMLMRMLMRM LMrMLMrMlM")]
        public void SendCollisionCommandToRoverShouldThrowInvalidCommandException(int xCoordinateRover1,
            int yCoordinateRover1,
            char headingRover1,
            int initialXCoordinateRover2,
            int initialYCoordinateRover2,
            char initialHeadingRover2,
            string commands)
        {
            // Arrange
            AddRoverToMap(xCoordinateRover1, yCoordinateRover1, headingRover1);
            AddRoverToMap(initialXCoordinateRover2, initialYCoordinateRover2, initialHeadingRover2);

            // Assert
            Should.Throw<InvalidCommandException>(() => target.SendCommands(commands));
        }

        [TestMethod]
        [DataRow(1, 2, 'N', "LLMMMMMMM")]
        [DataRow(0, 2, 'N', "Lm")]
        [DataRow(5, 5, 'N', "M")]
        public void SendOutOfBoundsCommandToRoverShouldThrowInvalidCommandException(int initialXCoordinate,
            int initialYCoordinate,
            char initialHeading,
            string commands)
        {
            // Arrange
            AddRoverToMap(initialXCoordinate, initialYCoordinate, initialHeading);

            // Assert
            Should.Throw<InvalidCommandException>(() => target.SendCommands(commands));
        }

        private void AddRoverToMap(int initialXCoordinate,
            int initialYCoordinate,
            char initialHeading)
        {
            var position = new Position { X = initialXCoordinate, Y = initialYCoordinate };
            var heading = initialHeading;

            target.InitiateRover(position, heading.ToString());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            RoverMap.Rovers = new List<Rover>();
            RoverMap.RoversLocation = new Dictionary<Position, Rover>();
        }
    }
}
