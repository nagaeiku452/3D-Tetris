using MainGame.Physics.StaticGridSystem;

namespace _3D_Tetris
{
    internal interface IThreeDimTetrisWallKickConfiguration<T> where T : TetrisBodyBase
    {
        public void WallKickTest(RotatableTetris curTetris, StaticGridDynamicWorld<T> world, StaticGridDirection rotationAxis, IThreeDimTetrisWallKickResult<T> wallKickResult);
    }
}