using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using _3D_Tetris.Drawing;

namespace _3D_Tetris.WinForm
{
    internal class WorldObjectPainter : IDisposable
    {
        private readonly BWBoxSprite boxSprite;
        private readonly WorldViewBox worldViewBox;
        private readonly ImageAttributes imageAttributes = new();
        private readonly ColorMatrix matrix = new();
        private readonly CameraViewPlane cameraViewPlane = new();

        public Vector3i ViewBoxMin
        {
            get => worldViewBox.Min;
            set => worldViewBox.Min = value;
        }

        public Vector3i ViewBoxMax
        {
            get => worldViewBox.Max;
            set => worldViewBox.Max = value;
        }
        public System.Drawing.Point UnitXProjection => new(boxSprite.ZSurfaceWidth / 2, -boxSprite.ZSurfaceHeight / 2);
        public System.Drawing.Point UnitYProjection => new(-boxSprite.ZSurfaceWidth / 2, -boxSprite.ZSurfaceHeight / 2);
        public System.Drawing.Point UnitZProjection => new(0, -boxSprite.BoxHeight);

        public CameraViewAngle CurrentViewAngle
        {
            get => cameraViewPlane.CurrentViewAngle;
            set => cameraViewPlane.CurrentViewAngle = value;
        }

        public WorldObjectPainter(BWBoxSprite sprite, WorldViewBox viewBox)
        {
            boxSprite = sprite;
            worldViewBox = viewBox;
        }

        public void ChangeViewBoxSize(Vector3i min, Vector3i max)
        {
            worldViewBox.Min = min;
            worldViewBox.Max = max;
        }

        public void ChangeSpriteSize(int boxHeight, int zSurfaceHeight, int zSurfaceWidth)
        {
            boxSprite.ResizeImage(boxHeight, zSurfaceHeight, zSurfaceWidth);
        }

        public void ChangeSurfaceBrightness(byte xBrightness, byte yBrightness, byte zBrightness)
        {
            boxSprite.AdjustSurfaceBrightness(xBrightness, yBrightness, zBrightness);
        }

        public void PaintWorld<T>(StaticGridDynamicWorld<T>? world, Graphics g, System.Drawing.Point anchor, CameraViewAngle viewAngle, BoxShader boxShader) where T : TetrisBodyBase
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (g == null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            foreach ((Drawing.Point, TetrisBodyBase) paintData in cameraViewPlane.GenerateCameraView(worldViewBox.WorldViewInterception(world), worldViewBox.Min, worldViewBox.Max, viewAngle, new Drawing.Point(boxSprite.ZSurfaceWidth / 2, boxSprite.ZSurfaceHeight / 2), new Drawing.Point(-boxSprite.ZSurfaceWidth / 2, boxSprite.ZSurfaceHeight / 2), new Drawing.Point(0, -boxSprite.BoxHeight)).Select(v => ((Drawing.Point, TetrisBodyBase))v))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //ColorMatrixGenerator.GenerateColorMatrix(Color.FromArgb(paintColor.A, (int)(gentColorInfo.Item1 * 255), (int)(gentColorInfo.Item2 * 255), (int)(gentColorInfo.Item3 * 255)), matrix);
                boxShader?.AddAdditionInfo(paintData.Item2, "targetTransform");
                ColorMatrixGenerator.GenerateColorMatrix(boxShader != null ? boxShader.ColorAdjust(paintData.Item2.PaintColor.ExplicitConvert()) : paintData.Item2.PaintColor.ExplicitConvert(), matrix);
                imageAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(boxSprite.Sprite!, new Rectangle(new System.Drawing.Point(anchor.X + paintData.Item1.X - boxSprite.ZSurfaceWidth / 2, anchor.Y + paintData.Item1.Y - boxSprite.BoxHeight), boxSprite.Sprite.Size), 0, 0, boxSprite.Sprite.Width, boxSprite.Sprite.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            //ColorMatrixGenerator.GenerateColorMatrix(kvp.Value, matrix);
            //    imageAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            //    g.DrawImage(boxSprite.Sprite, new Rectangle(default, boxSprite.Sprite.Size), anchor.X, anchor.Y, boxSprite.Sprite.Width, boxSprite.Sprite.Height, GraphicsUnit.Pixel, imageAttributes);
        }

        public void Dispose()
        {
            imageAttributes.Dispose();
        }
    }
}