namespace _3D_Tetris
{
    internal class ThreeDimTetrisScoreCounter
    {
        public int CurrentScore { get; private set; }

        public void InitNewScore()
        {
            CurrentScore = 0;
        }

        internal void OnSliceClear(int sliceCleared, int currentLevel)
        {
            if (sliceCleared < 0)
            {
                return;
            }
            int i = 50 * currentLevel + 50;
            while (sliceCleared > 0)
            {
                i *= 2;
                sliceCleared--;
            }

            CurrentScore += i;
        }

        internal void OnCurrentTetrisHardDropped(int droppedLength, int currentLevel)
        {
            if (droppedLength < 0 || currentLevel < 0) { return; }
            CurrentScore += 2 * droppedLength * currentLevel;
        }

        internal void OnCurrentTetrisSoftDropped(int currentLevel)
        {
            if (currentLevel < 0) { return; }
            CurrentScore += currentLevel;
        }
    }
}