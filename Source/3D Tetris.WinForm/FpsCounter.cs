using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris.WinForm
{
    internal class FpsCounter
    {
        public double CurrentFps { get; private set; }
        private int EventCount;
        private DateTime LastCheckedTime = DateTime.Now;
        private readonly TimeSpan OneSec = TimeSpan.FromSeconds(1);

        public FpsCounter() { }

        public void InvokeEvent()
        {
            if (DateTime.Now > LastCheckedTime + OneSec)
            {
                CurrentFps = EventCount * 1000 / (DateTime.Now-LastCheckedTime).TotalMilliseconds;
                EventCount = 0;
                LastCheckedTime += OneSec;
            }
            EventCount++;
        }
    }
}
