using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MainGame.Physics.StaticGridSystem;
using MainGame.Numeric;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Tetris
{
    internal sealed class RotatableTetris : TetrisBodyBase
    {
        private readonly ISet<Vector3i> checkedCoordinate = new HashSet<Vector3i>();
        private int shapeSize;

        public RotatableShapeWrapper RotatableShape = new();

        public override GridCollisionShape GridCollisionShape
        {
            get => RotatableShape?.CollisionShape;
            set
            {
                RotatableShape.CollisionShape = value;
                shapeSize = -1;
            }
        }

        private void CheckCoordinate()
        {
            checkedCoordinate.Clear();
            foreach ((SimpleGridCollisionShape, Vector3i) item in GridCollisionShape)
            {
                Debug.Assert(item.Item1.CollisionShapeType == GridCollisionShapeType.Box);
                Vector3i maxPoint = item.Item1.MaxPoint;
                Vector3i minPoint = item.Item1.MinPoint;
                for (int i = 0; i < maxPoint.X - minPoint.X + 1; i++)
                {
                    for (int j = 0; j < maxPoint.Y - minPoint.Y + 1; j++)
                    {
                        for (int k = 0; k < maxPoint.Z - minPoint.Z + 1; k++)
                        {
                            checkedCoordinate.Add(item.Item2 + minPoint + new Vector3i(i, j, k) + WorldTransform);
                        }
                    }
                }
            }
        }

        private RotatableTetris(StaticGridRigidBodyType bodyType) : base(bodyType)
        {

        }

        public RotatableTetris() : this(StaticGridRigidBodyType.Dynamic)
        {
            
        }

        public IEnumerable<Vector3i> UnitBoxDecomposition
        {
            get
            {
                CheckCoordinate();
                shapeSize = checkedCoordinate.Count;
                return checkedCoordinate;
            }
        }
        public int ShapeSize
        {
            get
            {
                if (shapeSize < 0)
                {
                    CheckCoordinate();
                }
                return shapeSize;
            }
        }

        public void Rotate(StaticGridDirection dir)
        {
            RotatableShape.Rotate(dir);
        }

        public void ToOriginRotation()
        {
            RotatableShape.ToOriginRotation();
        }
    }
}
