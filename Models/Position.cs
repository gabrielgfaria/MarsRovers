using System;

namespace Models
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Position;

            if (item == null)
            {
                return false;
            }

            return X.Equals(item.X) && Y.Equals(item.Y);
        }

        public override int GetHashCode()
        {
            return int.Parse(Math.Abs(X).ToString() + Math.Abs(Y).ToString());
        }
    }
}
