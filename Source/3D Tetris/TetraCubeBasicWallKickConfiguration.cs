using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System.Collections.Generic;

namespace _3D_Tetris
{
    internal class TetraCubeBasicWallKickConfiguration : IThreeDimTetrisWallKickConfiguration<TetrisBodyBase>
    {
        private static readonly List<Vector3i> wallKickTestOrder = new();
        private readonly TFOnlyCollisionResult collisionResult = new();
        public void WallKickTest(RotatableTetris curTetris, StaticGridDynamicWorld<TetrisBodyBase> world, StaticGridDirection rotationAxis, IThreeDimTetrisWallKickResult<TetrisBodyBase> wallKickResult)
        {
            if (rotationAxis == StaticGridDirection.NoDirection)
            {
                wallKickResult.AddResult(true, Vector3i.Zero);
                return;
            }

            curTetris.Rotate(rotationAxis);

            foreach (Vector3i item in wallKickTestOrder)
            {
                collisionResult.ResetResult();
                world.CollisionTest(curTetris, item, collisionResult);
                if (!collisionResult.HasCollision)
                {
                    curTetris.Rotate(rotationAxis.ToOppositeDirection());
                    wallKickResult.AddResult(true, item);
                    return;
                }
            }
            curTetris.Rotate(rotationAxis.ToOppositeDirection());
            wallKickResult.AddResult(false, Vector3i.Zero);
        }

        static TetraCubeBasicWallKickConfiguration()
        {
            wallKickTestOrder.Add(Vector3i.Zero);
            wallKickTestOrder.Add(Vector3i.UnitX);
            wallKickTestOrder.Add(Vector3i.UnitY);
            wallKickTestOrder.Add(-Vector3i.UnitX);
            wallKickTestOrder.Add(-Vector3i.UnitY);
            wallKickTestOrder.Add(2 * Vector3i.UnitX);
            wallKickTestOrder.Add(2 * Vector3i.UnitY);
            wallKickTestOrder.Add(2 * -Vector3i.UnitX);
            wallKickTestOrder.Add(2 * -Vector3i.UnitY);
        }
    }
}