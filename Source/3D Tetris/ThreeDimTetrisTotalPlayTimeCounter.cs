using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisTotalPlayTimeCounter
    {
        public TimeSpan TotalPlayTime { get; private set; }

        public void InitTotalPlayTime()
        {
            TotalPlayTime = new TimeSpan(0);
        }

        public void AddPlayTime(int milliseconds)
        {
            TotalPlayTime += new TimeSpan(0, 0, 0, 0, milliseconds);
        }
    }
}