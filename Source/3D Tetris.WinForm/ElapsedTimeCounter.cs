using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris.WinForm
{
    internal class ElapsedTimeCounter
    {
        private readonly double cycleTime;
        private double elapsedTime = 0;
        private readonly TimerElapsedEventArgs timerElapsedEventArgs = new();
        private DateTime lastCheckedTime = DateTime.Now;

        internal event EventHandler<TimerElapsedEventArgs>? TimerElapsed;

        public ElapsedTimeCounter(double cycleTime)
        {
            if (cycleTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cycleTime));
            }

            this.cycleTime = cycleTime;
        }

        public void UpdateTimer()
        {
            DateTime newTime = DateTime.Now;
            AddTime((newTime - lastCheckedTime).TotalMilliseconds);
            lastCheckedTime = newTime;
        }

        private void AddTime(double time)
        {
            elapsedTime += time;
            while (elapsedTime > cycleTime)
            {
                timerElapsedEventArgs.TimeElapsed = cycleTime;
                TimerElapsed?.Invoke(this, timerElapsedEventArgs);
                elapsedTime -= cycleTime;
            }
        }
    }
}
