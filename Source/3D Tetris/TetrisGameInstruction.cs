using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    enum TetrisGameInstruction
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
