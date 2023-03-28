namespace _3D_Tetris
{
    internal struct SliceClearedEventArgs
    {
        public readonly int SliceCleared;

        public SliceClearedEventArgs(int sliceCleared)
        {
            SliceCleared = sliceCleared;
        }
    }
}