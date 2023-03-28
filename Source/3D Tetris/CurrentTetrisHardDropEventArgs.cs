namespace _3D_Tetris
{
    internal struct CurrentTetrisHardDropEventArgs
    {
        public int DroppedLength;

        public CurrentTetrisHardDropEventArgs(int droppedLength)
        {
            DroppedLength = droppedLength;
        }
    }
}