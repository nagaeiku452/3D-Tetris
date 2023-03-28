using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisSoftDropIntervalCounter
    {
        public EventHandler<EventArgs> SoftDropIntervalCounterTimeUp;
        private int countDownThreshold = -1;

        public void Reset()
        {
            countDownThreshold = GameConfigData.TetrisSoftDropInterval;
        }
        public void Tick()
        {
            if (countDownThreshold >= 0)
            {
                countDownThreshold--;
                if (countDownThreshold == 0)
                {
                    SoftDropIntervalCounterTimeUp?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}