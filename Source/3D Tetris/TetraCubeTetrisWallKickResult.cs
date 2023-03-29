using MainGame.Numeric;

namespace _3D_Tetris
{
    internal class TetraCubeTetrisWallKickResult<T> : IThreeDimTetrisWallKickResult<T> where T : TetrisBodyBase
    {
        public bool CanRotate { get; private set; }
        public Vector3i TransformOffset { get; private set; }

        public void AddResult(bool canRotate, Vector3i transformOffset)
        {
            CanRotate = canRotate;
            TransformOffset = transformOffset;
        }

        public void ResetResult()
        {
            CanRotate = false;
            TransformOffset = Vector3i.Zero;
        }
    }
}
