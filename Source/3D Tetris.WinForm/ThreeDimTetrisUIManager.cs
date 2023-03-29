using System;
using System.Drawing;

namespace _3D_Tetris.WinForm
{
    internal class ThreeDimTetrisUIManager
    {
        private readonly Color BackGroundColor = GameConfigData.BackgroundColor;
        private readonly MainWindow mainWindow;
        public ThreeDimTetrisUIManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            mainWindow.BackColor = BackGroundColor;
            mainWindow.Score.BackColor = BackGroundColor;
            mainWindow.Score.ForeColor = Color.White;
            mainWindow.Next.BackColor = BackGroundColor;
            mainWindow.Next.ForeColor = Color.White;
            mainWindow.Hold.BackColor = BackGroundColor;
            mainWindow.Hold.ForeColor = Color.White;
            mainWindow.SliceCleared.BackColor = BackGroundColor;
            mainWindow.SliceCleared.ForeColor = Color.White;
            mainWindow.Level.BackColor = BackGroundColor;
            mainWindow.Level.ForeColor = Color.White;
            mainWindow.PlayedTime.BackColor = BackGroundColor;
            mainWindow.PlayedTime.ForeColor = Color.White;
            mainWindow.TotalTetrisDropped.BackColor = BackGroundColor;
            mainWindow.TotalTetrisDropped.ForeColor = Color.White;
        }

        public void ShowScore(int currentScore)
        {
            mainWindow.Score.Text = $"Score: {currentScore}";
        }

        public void ShowSliceCleared(int sliceCleared)
        {
            mainWindow.SliceCleared.Text = $"Slice cleared: {sliceCleared}";
        }

        public void ShowLevel(int level)
        {
            mainWindow.Level.Text = $"Level: {level}";
        }

        internal void ShowPlayTime(TimeSpan totalPlayTime)
        {
            mainWindow.PlayedTime.Text = $"time played: {(int)totalPlayTime.TotalHours:D2}:{totalPlayTime.Minutes:D2}:{totalPlayTime.Seconds:D2}";
        }

        internal void ShowTetrisDropped(int totalTetrisDropped)
        {
            mainWindow.TotalTetrisDropped.Text = $"Tetris dropped: {totalTetrisDropped:D3}";
        }
    }
}
