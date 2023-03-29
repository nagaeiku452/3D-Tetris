using MainGame.Numeric;

namespace _3D_Tetris.WinForm
{
    internal class WorldViewBoxShader : BoxShader
    {
        private Vector3i viewBoxMin;
        private Vector3i viewBoxMax;
        private Vector3i targetTransform;

        public override void AddAdditionInfo<T>(T info, string tag)
        {
            if (tag == "viewBoxMin" && info is Vector3i min)
            {
                viewBoxMin = min;
            }
            else if (tag == "viewBoxMax" && info is Vector3i max)
            {
                viewBoxMax = max;
            }
            else if (tag == "targetTransform" && info is Vector3i targetTransform)
            {
                this.targetTransform = targetTransform;
            }
        }

        public override Color ColorAdjust(Color oriColor)
        {
            (float, float, float) hslInfo = RGBtoHSLConverter.RGBToHSL(oriColor.R / 255f, oriColor.G / 255f, oriColor.B / 255f);
            float zInterplation = viewBoxMax.Z - viewBoxMin.Z != 0 ? (targetTransform.Z - viewBoxMin.Z) / (float)(viewBoxMax.Z - viewBoxMin.Z) : 1;
            (float, float, float) gentColorInfo = RGBtoHSLConverter.HSLToRGB(hslInfo.Item1, hslInfo.Item2 * (0.6f + 0.25f * zInterplation), hslInfo.Item3 * (1.05f + 0.25f * zInterplation) > 1f ? 1f : hslInfo.Item3 * (1.05f + 0.25f * zInterplation));
            return Color.FromArgb(oriColor.A, (int)(gentColorInfo.Item1 * 255), (int)(gentColorInfo.Item2 * 255), (int)(gentColorInfo.Item3 * 255));
        }
    }
}
