using System;

namespace _3D_Tetris.Drawing
{
    public readonly struct Color
    {
        public byte A { get; }
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public static Color Transparent { get; } = FromArgb(0, 0, 0, 0);

        public Color(System.Drawing.Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }

        private Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        //public System.Drawing.Color GetColor => System.Drawing.Color.FromArgb(A, R, G, B);


        internal static Color FromArgb(int a, int r, int g, int b)
        {
            CheckByte(a);
            CheckByte(r);
            CheckByte(g);
            CheckByte(b);
            return new((byte)a, (byte)r, (byte)g, (byte)b);
        }

        internal static Color FromArgb(int r, int g, int b)
        {
            return FromArgb(r, g, b);
        }

        private static void CheckByte(int a)
        {
            if (a < 0 || a > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }
        }

        internal static Color FromRgb(byte backgroundColorR, byte backgroundColorG, byte backgroundColorB)
        {
            return FromArgb(byte.MaxValue, backgroundColorR, backgroundColorG, backgroundColorB);
        }

        public static implicit operator System.Drawing.Color(Color c) => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        public static implicit operator Color(System.Drawing.Color c) => FromArgb(c.A, c.R, c.G, c.B);
    }
}
