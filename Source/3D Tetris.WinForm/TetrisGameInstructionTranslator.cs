namespace _3D_Tetris.WinForm
{
    internal static class TetrisGameInstructionTranslator
    {
        static TetrisGameInstructionTranslator()
        {
        }

        internal static TetrisGameInstruction GetInstruction(Keys keyCode)
        {
            return keyCode switch
            {
                Keys.Q => TetrisGameInstruction.CounterClockWiseCameraRotate,
                Keys.E => TetrisGameInstruction.ClockWiseCameraRotate,
                Keys.A => TetrisGameInstruction.NegXRotation,
                Keys.S => TetrisGameInstruction.NegYRotation,
                Keys.D => TetrisGameInstruction.NegZRotation,
                Keys.Z => TetrisGameInstruction.PosXRotation,
                Keys.X => TetrisGameInstruction.PosYRotation,
                Keys.C => TetrisGameInstruction.PosZRotation,
                Keys.ShiftKey => TetrisGameInstruction.Hold,
                Keys.Space => TetrisGameInstruction.HardDrop,
                Keys.Up => TetrisGameInstruction.MoveUpRight,
                Keys.Left => TetrisGameInstruction.MoveUpLeft,
                Keys.Down => TetrisGameInstruction.MoveDownLeft,
                Keys.Right => TetrisGameInstruction.MoveDownRight,
                Keys.NumPad0 => TetrisGameInstruction.SoftDrop,
                _ => TetrisGameInstruction.None,
            };
        }
    }
}
