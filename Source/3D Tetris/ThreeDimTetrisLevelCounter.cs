using System;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisLevelCounter
    {
        public int CurrentLevel
        {
            get; private set;
        } = GameConfigData.InitialLevel;
        public void InitNewCounter()
        {
            CurrentLevel = GameConfigData.InitialLevel;
        }

        public void AdjustCurrentLevel(int totalSliceCleared)
        {
            if (totalSliceCleared < 1)
            {
                CurrentLevel = GameConfigData.InitialLevel;
            }
            else
            {
                CurrentLevel = Math.Max(totalSliceCleared / GameConfigData.ClearedSlicesRequiredForLevelUp + 1, GameConfigData.InitialLevel);
            }
        }
    }
}