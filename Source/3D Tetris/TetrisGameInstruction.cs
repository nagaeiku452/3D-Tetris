namespace _3D_Tetris
{
    public enum TetrisGameInstruction
    {
        None = 0,
        MoveUpLeft,
        MoveUpRight,
        MoveDownLeft,
        MoveDownRight,
        PosXRotation,
        PosYRotation,
        PosZRotation,
        NegXRotation,
        NegYRotation,
        NegZRotation,
        ClockWiseCameraRotate,
        CounterClockWiseCameraRotate,
        SoftDrop,
        HardDrop,
        Hold
    }
}
