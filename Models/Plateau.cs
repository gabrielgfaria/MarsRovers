namespace Models
{
    public static class Plateau
    {
        public static Position Boundaries { get; set; }

        public static void SetBoundaries(Position boundaries)
        {
            Boundaries = boundaries;
        }
    }
}
