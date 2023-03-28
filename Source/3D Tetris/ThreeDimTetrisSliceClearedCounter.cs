using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisSliceClearedCounter
    {
        public int TotalSliceCleared { get; private set; }
        public void InitNewCounter()
        {
            TotalSliceCleared = 0;
        }

        internal void OnSliceClear(int sliceCleared)
        {
            TotalSliceCleared += sliceCleared;
        }
    }
}