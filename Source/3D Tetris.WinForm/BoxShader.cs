namespace _3D_Tetris.WinForm
{
    internal abstract class BoxShader
    {
        public abstract void AddAdditionInfo<T>(T info, string tag);
        public abstract Color ColorAdjust(Color oriColor);
    }
}
