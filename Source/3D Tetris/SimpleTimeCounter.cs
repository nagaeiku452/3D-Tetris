using System;

namespace _3D_Tetris
{
    internal class SimpleTimeCounter
    {
        public event EventHandler<EventArgs> OnCountDownEnd;
        protected int countDownThreshold = -1;

        public void Reset(int threshold)
        {
            countDownThreshold = threshold;
        }
        public void Tick()
        {
            if (countDownThreshold >= 0)
            {
                countDownThreshold--;
                if (countDownThreshold == 0)
                {
                    OnCountDownEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}