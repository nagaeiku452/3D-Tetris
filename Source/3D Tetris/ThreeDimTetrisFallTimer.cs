using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisFallTimer:IDisposable
    {
        private readonly ThreeDimTetrisFallIntervalCounter intervalCounter = new();
        private readonly ThreeDimTetrisSoftDropIntervalCounter softDropIntervalCounter = new();

        public EventHandler<FallEventArgs> TetrisFall;
        private bool softDropEnabled;
        private bool disposedValue;

        public bool SoftDropEnabled
        {
            get => softDropEnabled;
            set
            {
                if (!softDropEnabled&& value)
                {
                    softDropIntervalCounter.Reset();
                }
                softDropEnabled = value;
            }
        }

        public ThreeDimTetrisFallTimer()
        {
            intervalCounter.FallIntervalCounterTimeUp += OnIntervalTimeUp;
            softDropIntervalCounter.SoftDropIntervalCounterTimeUp += OnIntervalTimeUp;
            softDropIntervalCounter.Reset();
        }

        private void OnIntervalTimeUp(object sender, EventArgs e)
        {
            TetrisFall?.Invoke(this,new FallEventArgs(softDropEnabled));
            intervalCounter.Reset();
            softDropIntervalCounter.Reset();
        }

        public void Tick()
        {
            intervalCounter.Tick();
            if (softDropEnabled)
            {
                softDropIntervalCounter.Tick();
            }
        }

        public void ResetFallInterval(int currentLevel)
        {
            intervalCounter.ResetFallInterval(currentLevel);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)

                    intervalCounter.FallIntervalCounterTimeUp -= OnIntervalTimeUp;
                    softDropIntervalCounter.SoftDropIntervalCounterTimeUp -= OnIntervalTimeUp;
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~ThreeDimTetrisFallTimer()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
