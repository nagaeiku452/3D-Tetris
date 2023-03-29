namespace _3D_Tetris.WinForm
{
    internal static class ColorExtension
    {
        public static Color ExplicitConvert(this Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
