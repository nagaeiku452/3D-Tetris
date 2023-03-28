using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisUIManager
    {
        private static readonly Color BackGroundColor = Color.FromArgb(GameConfigData.BackgroundColorR, GameConfigData.BackgroundColorG, GameConfigData.BackgroundColorB);
        private MainWindow mainWindow;
        public ThreeDimTetrisUIManager()
        {

        }

        public void Initialize(MainWindow mainWindow)
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
