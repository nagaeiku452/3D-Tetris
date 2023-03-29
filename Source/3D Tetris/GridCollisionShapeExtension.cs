using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;

namespace _3D_Tetris
{
    internal static class GridCollisionShapeExtension
    {
        public static GridCollisionShape CloneShape(this GridCollisionShape otherShape)
        {
            if (otherShape is MultiGridCollisionShape multiShape)
            {
                MultiGridCollisionShape cloneShape = new(null);
                foreach ((SimpleGridCollisionShape, Vector3i) item in multiShape)
                {
                    cloneShape.AddLeafShape(CloneSimpleShape(item.Item1), item.Item2);
                }
                return cloneShape;
            }
            return CloneSimpleShape((SimpleGridCollisionShape)otherShape);
        }

        private static SimpleGridCollisionShape CloneSimpleShape(SimpleGridCollisionShape otherShape)
        {
            if (otherShape is GridBoxCollisionShape boxShape)
            {
                return new GridBoxCollisionShape(boxShape.MinPoint, boxShape.MaxPoint);
            }
            else if (otherShape is GridSingleInclineBoxCollisionShape singleInclineShape)
            {
                return new GridSingleInclineBoxCollisionShape(singleInclineShape.InclineSurfaceNormalDirection);
            }
            else if (otherShape is GridLongInclineBoxCollisionShape longInclineShape)
            {
                return new GridLongInclineBoxCollisionShape(longInclineShape.MinPosition, longInclineShape.MaxPosition, longInclineShape.PositiveDirection, longInclineShape.InclineSurfaceNormalDirection);
            }
            return null;
        }
    }
}
