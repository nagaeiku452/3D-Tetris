
namespace _3D_Tetris.WinForm
{
    internal class SingleModelBoxShader : BoxShader
    {
        private bool IsHeld;
        public override void AddAdditionInfo<T>(T info, string s)
        {
            if (info is bool b && s == "Held")
            {
                IsHeld = b;
            }
        }

        public override Color ColorAdjust(Color oriColor)
        {
            (float, float, float) hslInfo = RGBtoHSLConverter.RGBToHSL(oriColor.R / 255f, oriColor.G / 255f, oriColor.B / 255f);
            (float, float, float) gentColorInfo = RGBtoHSLConverter.HSLToRGB(hslInfo.Item1, hslInfo.Item2 * 0.8f, hslInfo.Item3 * 1.2f > 1f ? 1f : hslInfo.Item3 * 1.2f);
            return Color.FromArgb(IsHeld ? (int)(oriColor.A * 0.24f) : oriColor.A, (int)(gentColorInfo.Item1 * 255), (int)(gentColorInfo.Item2 * 255), (int)(gentColorInfo.Item3 * 255)); ;
        }
    }
}
