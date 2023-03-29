using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using MainGame.Physics.Utilities;
using _3D_Tetris.Drawing;

namespace _3D_Tetris
{
    class TetrisMorphling
    {
        public readonly RotatableTetris Instance;
        private readonly MultiBoxShapeReplicator shapeReplicator = new();
        public int TetrisNum { get; private set; } = -1;

        public TetrisMorphling()
        {
            Instance = new RotatableTetris()
            {
                GridCollisionShape = new MultiGridCollisionShape(),
            };
        }

        private void CloneMultiBoxShape(GridCollisionShape shape)
        {
            Instance.ToOriginRotation();
            shapeReplicator.DestructClonedShape(Instance.GridCollisionShape);
            shapeReplicator.CloneMultiBoxShape(Instance.GridCollisionShape, shape);
        }

        public void CloneTetrisData((GridCollisionShape, Color) data, int num)
        {
            CloneMultiBoxShape(data.Item1);
            Instance.PaintColor = data.Item2;
            TetrisNum = num;
        }

        private class MultiBoxShapeReplicator
        {
            private readonly InstancePrototypeContainer<BoxShapeInstancePrototype> boxShapeContainer = new();

            public void CloneMultiBoxShape(GridCollisionShape targetShape, GridCollisionShape otherShape)
            {
                if(targetShape is MultiGridCollisionShape shapeBuffer)
                {
                    shapeBuffer.ClearAllLeafShape();

                    foreach ((SimpleGridCollisionShape, Vector3i) item in otherShape)
                    {
                        if (item.Item1 is GridBoxCollisionShape)
                        {
                            BoxShapeInstancePrototype newInstance = boxShapeContainer.RentPrototypeInstance();
                            newInstance.ResetSize(item.Item1.MinPoint, item.Item1.MaxPoint);
                            shapeBuffer.AddLeafShape(newInstance, item.Item2);
                        }
                    }
                }
            }

            public void DestructClonedShape(GridCollisionShape targetShape)
            {
                if (targetShape is MultiGridCollisionShape shapeBuffer)
                {
                    foreach ((SimpleGridCollisionShape, Vector3i) item in shapeBuffer)
                    {
                        boxShapeContainer.ReturnInstance(item.Item1 as BoxShapeInstancePrototype);
                    }
                    shapeBuffer.ClearAllLeafShape();
                }
            }

            private class BoxShapeInstancePrototype : GridBoxCollisionShape, IInstancePrototype
            {
                public BoxShapeInstancePrototype() : base(Vector3i.Zero, Vector3i.Zero) { }

                public void Clear()
                {
                    ResetSize(Vector3i.Zero, Vector3i.Zero);
                }
            }
        }
    }
}
