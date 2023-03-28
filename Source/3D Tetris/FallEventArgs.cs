namespace _3D_Tetris
{
    internal readonly struct FallEventArgs
    {
        public readonly bool IsSoftDrop;

        public FallEventArgs(bool isSoftDrop)
        {
            IsSoftDrop = isSoftDrop;
        }
    }
}