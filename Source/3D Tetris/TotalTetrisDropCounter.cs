namespace _3D_Tetris
{
    class TotalTetrisDropCounter
    {
        public int TotalTetrisDropped { get; private set; }
        public void Init()
        {
            TotalTetrisDropped = 0;
        }

        public void Dropped()
        {
            TotalTetrisDropped += 1;
        }
    }
}
