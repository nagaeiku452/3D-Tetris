﻿namespace _3D_Tetris.WinForm
{
    internal class BWBoxSprite : IDisposable
    {
        private byte xBrightness;
        private byte yBrightness;
        private byte zBrightness;

        public Image Sprite { get; private set; } = null!;
        public int BoxHeight { get; private set; }
        public int ZSurfaceHeight { get; private set; }
        public int ZSurfaceWidth { get; private set; }

        // do nothing if sprite is null
        public void ResizeImage(int boxHeight, int zSurfaceHeight, int zSurfaceWidth)
        {
            if (Sprite == null) { return; }
            BoxHeight = boxHeight;
            ZSurfaceHeight = zSurfaceHeight;
            ZSurfaceWidth = zSurfaceWidth;
            GenerateImage();
        }
        // do nothing if sprite is null

        public void AdjustSurfaceBrightness(byte xBrightness, byte yBrightness, byte zBrightness)
        {
            if (Sprite == null) { return; }
            this.xBrightness = xBrightness;
            this.yBrightness = yBrightness;
            this.zBrightness = zBrightness;
            GenerateImage();
        }

        public void GenerateImage(int boxHeight, int zSurfaceHeight, int zSurfaceWidth, byte xBrightness, byte yBrightness, byte zBrightness)
        {
            BoxHeight = boxHeight;
            ZSurfaceHeight = zSurfaceHeight;
            ZSurfaceWidth = zSurfaceWidth;
            this.xBrightness = xBrightness;
            this.yBrightness = yBrightness;
            this.zBrightness = zBrightness;
            GenerateImage();
        }

        private void GenerateImage()
        {
            Sprite = BWBoxSpriteGenerator.GenerateBWBoxSprite(ZSurfaceWidth, ZSurfaceHeight + BoxHeight, BoxHeight, xBrightness, yBrightness, zBrightness);
        }

        public void Dispose()
        {
            Sprite?.Dispose();
        }
    }
}
