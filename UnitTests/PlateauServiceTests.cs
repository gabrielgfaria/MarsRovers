using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using Services.Exceptions;
using Shouldly;

namespace UnitTests
{
    [TestClass]
    public class PlateauServiceTests
    {
        private readonly IPlateuService _target;

        public PlateauServiceTests()
        {
            _target = new PlateuService();
        }

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(100000, 2)]
        [DataRow(0, 0)]
        public void SettingBoundariesWithValidValuesShouldSetBoundaries(int xBoundary, int yBoundary)
        {
            // Arrange
            var boundaries = (xBoundary, yBoundary);

            // Act
            _target.EstablishBoundaries(boundaries);

            // Assert
            Plateau.Boundaries.ShouldBe(boundaries);
        }

        [TestMethod]
        [DataRow(-1, 5)]
        [DataRow(15, -2)]
        [DataRow(-10, -1)]
        public void SettingBoundariesWithInvalidValuesShouldSetBoundaries(int xBoundary, int yBoundary)
        {
            // Arrange
            var boundaries = (xBoundary, yBoundary);

            // Assert
            Should.Throw<InvalidBoundariesException>(() => _target.EstablishBoundaries(boundaries));
        }
    }
}
