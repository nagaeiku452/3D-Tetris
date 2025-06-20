﻿namespace _3D_Tetris.WinForm
{
    internal abstract class WinFormControlUnit : IDisposable
    {
        protected readonly MainWindow mainWindow;

        public virtual void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        public virtual void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
        public virtual void OnGameLoopEvent()
        {

        }

        public virtual void OnRenderLoopEvent()
        {

        }

        protected WinFormControlUnit(MainWindow callerForm)
        {
            mainWindow = callerForm;
        }

        public abstract void Dispose();

        public virtual void OnMouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
