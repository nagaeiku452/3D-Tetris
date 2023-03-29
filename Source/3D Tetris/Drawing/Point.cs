namespace _3D_Tetris.Drawing
{
    public readonly struct Point
    {
        public Point(int v1, int v2)
        {
            X = v1;
            Y = v2;
        }

        public int X { get; }
        public int Y { get; }


        public static implicit operator System.Drawing.Point(Point c) => new(c.X, c.Y);
        public static implicit operator Point(System.Drawing.Point c) => new(c.X, c.Y);
    }
}
