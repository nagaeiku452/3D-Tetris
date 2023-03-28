using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class BWBoxSpriteGenerator
    {
        private readonly static SolidBrush paintBrush = new(Color.White);
        private static Graphics g;

        //Return a new object if operation success ,otherwise ,null.
        internal static Image GenerateBWBoxSprite(int imageWidth, int imageHeight, int boxHeight, int xSurfaceLightness, int ySurfaceLightness, int zSurfaceLightness)
        {
            if (imageWidth < 1 || boxHeight < 1 || imageHeight < boxHeight || xSurfaceLightness < 0 || xSurfaceLightness > 255 || ySurfaceLightness < 0 || ySurfaceLightness > 255 || zSurfaceLightness < 0 || zSurfaceLightness > 255)
            {
                return null;
            }

            Color xSurfaceColor = Color.FromArgb(xSurfaceLightness, xSurfaceLightness, xSurfaceLightness);
            Color ySurfaceColor = Color.FromArgb(ySurfaceLightness, ySurfaceLightness, ySurfaceLightness);
            Color zSurfaceColor = Color.FromArgb(zSurfaceLightness, zSurfaceLightness, zSurfaceLightness);

            //float zSurfaceHeight = (float)(Math.Sin(elevationRadius) * imageWidth);
            //float boxHeight = (int)(Math.Cos(elevationRadius) * imageWidth);
            int zSurfaceHeight = imageHeight - boxHeight;
            Image Canvas = new Bitmap(imageWidth, imageHeight);
            g = Graphics.FromImage(Canvas);
            g.Clear(Color.Transparent);

            paintBrush.Color = zSurfaceColor;
            Point[] pointsBuffer = new Point[4] { new Point(imageWidth / 2, 0), new Point(imageWidth, zSurfaceHeight / 2), new Point(imageWidth / 2, zSurfaceHeight), new Point(0, zSurfaceHeight / 2) };
            g.FillPolygon(paintBrush, pointsBuffer);

            paintBrush.Color = xSurfaceColor;
            pointsBuffer[0] = new Point(imageWidth, zSurfaceHeight / 2);
            pointsBuffer[1] = new Point(imageWidth / 2, zSurfaceHeight);
            pointsBuffer[2] = new Point(imageWidth / 2, zSurfaceHeight + boxHeight);
            pointsBuffer[3] = new Point(imageWidth, (zSurfaceHeight / 2) + boxHeight);
            g.FillPolygon(paintBrush, pointsBuffer);

            paintBrush.Color = ySurfaceColor;
            pointsBuffer[0] = new Point(0, zSurfaceHeight / 2);
            pointsBuffer[1] = new Point(imageWidth / 2, zSurfaceHeight);
            pointsBuffer[2] = new Point(imageWidth / 2, zSurfaceHeight + boxHeight);
            pointsBuffer[3] = new Point(0, (zSurfaceHeight / 2) + boxHeight);
            g.FillPolygon(paintBrush, pointsBuffer);

            return Canvas;
        }
    }
}
