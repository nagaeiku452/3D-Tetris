using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisFallIntervalCounter
    {
        public EventHandler<EventArgs> FallIntervalCounterTimeUp;
        private static readonly int levelOneIntervalInternal = CaculateInitInterval(1);
        private int countDownThreshold = CalibratedInterval(0);
        private int countDownThresholdPreset = CalibratedInterval(0);

        public void ResetFallInterval(int currentLevel)
        {
            countDownThreshold = countDownThresholdPreset = CalibratedInterval(currentLevel);
        }

        public void Tick()
        {
            countDownThreshold--;
            if (countDownThreshold == 0)
            {
                FallIntervalCounterTimeUp?.Invoke(this, EventArgs.Empty);
            }
        }

        private static int CaculateInitInterval(int currentLevel)
        {
            int maxLvl = Math.Min(currentLevel, 16);
            int a = (maxLvl * (maxLvl - 31)) + 244;
            return a / 2;
        }

        private static int CalibratedInterval(int currentLevel)
        {
            return CaculateInitInterval(currentLevel) * GameConfigData.TetrisFallInterval / levelOneIntervalInternal;
        }

        internal void Reset()
        {
            countDownThreshold = countDownThresholdPreset;
        }
    }
}