using MainGame.Numeric;

namespace _3D_Tetris
{
    internal interface IThreeDimTetrisWallKickResult<T> where T : TetrisBodyBase
    {
        public void AddResult(bool canRotate, Vector3i transformOffset);
    }
}