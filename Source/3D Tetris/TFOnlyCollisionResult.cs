using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class TFOnlyCollisionResult : ICollisionResult<TetrisBodyBase>
    {

        public bool HasCollision { get; private set; }
        public void AddSingleResult(ShapeWrapper myShape, StaticGridCollisionObjectWrapper<TetrisBodyBase> otherObject, CollisionResultType resultType, StaticGridDirection contactDirection)
        {
            if (resultType == CollisionResultType.HasCollision)
            {
                HasCollision = true;
            }
        }

        public void ResetResult()
        {
            HasCollision = false;
        }
    }
}
