﻿using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisFallIntervalCounter : SimpleTimeCounter
    {
        private int countDownThresholdPreset = CaculateInitInterval(1);

        public void ResetFallInterval(int currentLevel)
        {
            countDownThreshold = countDownThresholdPreset = CaculateInitInterval(currentLevel);
        }

        internal ThreeDimTetrisFallIntervalCounter()
        {
            countDownThreshold = CaculateInitInterval(1);
        }

        private static int CaculateInitInterval(int currentLevel)
        {
            int m = currentLevel - 1;
            double f = GameConfigData.TetrisFallInterval;
            for (int i = 0; i < currentLevel - 1; i++)
            {
                f *= 0.8 + 0.003 * m;
            }
            return (int)f;
        }


        internal void Reset()
        {
            countDownThreshold = countDownThresholdPreset;
        }
    }
}