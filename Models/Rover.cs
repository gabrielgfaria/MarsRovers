using System;

namespace Models
{
    public class Rover
    {
        public string Id { get; set; }
        public Position Position { get; set; }
        public char Heading { get; set; }

        public string PositionAndHeading()
        {
            return $"{Position} {Heading}";
        }
    }
}
