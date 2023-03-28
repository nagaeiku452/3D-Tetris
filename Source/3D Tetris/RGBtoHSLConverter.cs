using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    public static class RGBtoHSLConverter
    {
        public static (float, float, float) HSLToRGB(float h, float s, float l)
        {
            float var2 = l < 0.5f ? l * (1 + s) : l + s - l * s;
            float var1 = 2 * l - var2;
            return (HueToRGB(var1, var2, h + 1 / 3f), HueToRGB(var1, var2, h), HueToRGB(var1, var2, h - 1 / 3f));
        }

        public static (float, float, float) HSLToRGB((float, float, float) hslInfo)
        {
            return HSLToRGB(hslInfo.Item1, hslInfo.Item2, hslInfo.Item3);
        }

        private static float HueToRGB(float v1, float v2, float vH)
        {
            if (vH < 0 || vH > 1)
            {
                vH -= (float)Math.Floor(vH);
            }
            if (6 * vH < 1) { return v1 + (v2 - v1) * 6 * vH; }
            else if (2 * vH < 1) { return v2; }
            else if (3 * vH < 2) { return v1 + (v2 - v1) * 6 * (2 / 3f - vH); }
            return v1;
        }

        public static (float, float, float) RGBToHSL(float r, float g, float b)
        {
            float minColor = r > b ? b : r;
            minColor = minColor > g ? g : minColor;
            float maxColor = r > b ? r : b;
            maxColor = maxColor > g ? maxColor : g;
            float deltaMax = maxColor - minColor;

            float l = (maxColor + minColor) / 2;
            if (deltaMax == 0f)
            {
                return (0f, 0f, l);
            }
            float s = l < 0.5f ? deltaMax / (maxColor + minColor) : deltaMax / (2 - maxColor - minColor);

            float h;
            if (maxColor == r)
            {
                h = (g - b) / deltaMax + (g < b ? 6 : 0);
            }
            else if (maxColor == g)
            {
                h = (b - r) / deltaMax + 2;
            }
            else
            {
                h = (r - g) / deltaMax + 4;
            }
            return (h / 6f, s, l);
        }

        public static (float, float, float) RGBToHSL((float, float, float) rgbInfo)
        {
            return RGBToHSL(rgbInfo.Item1, rgbInfo.Item2, rgbInfo.Item3);
        }
    }
}
