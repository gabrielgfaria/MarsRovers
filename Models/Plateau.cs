namespace Models
{
    public static class Plateau
    {
        public static (int X, int Y) Boundaries { get; set; }

        public static void SetBoundaries((int x, int y) boundaries)
        {
            Boundaries = boundaries;
        }
    }
}
