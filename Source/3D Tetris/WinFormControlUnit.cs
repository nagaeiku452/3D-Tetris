using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D_Tetris
{
    internal abstract class WinFormControlUnit : IDisposable
    {
        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        public virtual void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
        public virtual void OnGameLoopEvent(object sender, EventArgs e)
        {

        }

        public virtual void OnRenderLoopEvent(object sender, EventArgs e)
        {

        }

        public abstract void InitControlUnit(MainWindow callerForm);

        public abstract void Dispose();

        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
