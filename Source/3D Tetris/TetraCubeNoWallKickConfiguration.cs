using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;

namespace _3D_Tetris
{
    internal class TetraCubeNoWallKickConfiguration : IThreeDimTetrisWallKickConfiguration<TetrisBodyBase>
    {
        private readonly TFOnlyCollisionResult collisionResult = new();
        public void WallKickTest(RotatableTetris curTetris, StaticGridDynamicWorld<TetrisBodyBase> world, StaticGridDirection rotationAxis, IThreeDimTetrisWallKickResult<TetrisBodyBase> wallKickResult)
        {
            if (rotationAxis == StaticGridDirection.NoDirection)
            {
                wallKickResult.AddResult(true, Vector3i.Zero);
                return;
            }

            curTetris.Rotate(rotationAxis);
            collisionResult.ResetResult();
            world.CollisionTest(curTetris, Vector3i.Zero, collisionResult);
            curTetris.Rotate(rotationAxis.ToOppositeDirection());
            wallKickResult.AddResult(!collisionResult.HasCollision, Vector3i.Zero);
        }
    }
}