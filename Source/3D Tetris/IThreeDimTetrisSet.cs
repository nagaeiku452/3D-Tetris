using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal interface IThreeDimTetrisSet
    {
        public int MaximumTetrisSize { get; }
        public int NextTetrisNum { get; }
        public int TotalTetrisVaries { get; }

        public (GridCollisionShape, Color) GetTetrisData(int num);
    }
}
