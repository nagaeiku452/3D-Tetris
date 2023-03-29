using MainGame.Physics.StaticGridSystem;
using _3D_Tetris.Drawing;

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
