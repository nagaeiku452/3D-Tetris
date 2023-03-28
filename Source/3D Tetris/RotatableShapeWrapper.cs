using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class RotatableShapeWrapper
    {
        private GridCollisionShape collisionShape;
        private readonly ICollection<(SimpleGridCollisionShape, Vector3i)> multiBoxShapes = new List<(SimpleGridCollisionShape, Vector3i)>();
        private StaticGridRotation currentRotation = StaticGridRotation.Identity;

        public GridCollisionShape CollisionShape
        {
            get => collisionShape;
            set
            {
                ToOriginRotation();
                collisionShape = value;
            }
        }


        public void Rotate(StaticGridDirection dir)
        {
            Rotate(new StaticGridRotation(dir));
        }
        public void Rotate(StaticGridRotation rotation)
        {
            if (collisionShape is SimpleGridCollisionShape simpleShape)
            {
                RotateSimpleShape(simpleShape, rotation);
            }
            else if (collisionShape is MultiGridCollisionShape multiShape)
            {
                multiBoxShapes.Clear();
                foreach ((SimpleGridCollisionShape, Vector3i) item in multiShape)
                {
                    RotateSimpleShape(item.Item1, rotation);
                    multiBoxShapes.Add((item.Item1, rotation * item.Item2));
                }
                multiShape.ClearAllLeafShape();
                foreach ((SimpleGridCollisionShape, Vector3i) item in multiBoxShapes)
                {
                    multiShape.AddLeafShape(item.Item1, item.Item2);
                }
            }
            currentRotation = rotation * currentRotation;
        }

        private static void RotateSimpleShape(SimpleGridCollisionShape simpleShape, StaticGridRotation rotation)
        {
            if (simpleShape is GridBoxCollisionShape boxShape)
            {
                RotateBoxShape(boxShape, rotation);
            }
            else if (simpleShape is GridSingleInclineBoxCollisionShape inclineShape)
            {
                RotateSingleInclineShape(inclineShape, rotation);
            }
        }

        private static void RotateSingleInclineShape(GridSingleInclineBoxCollisionShape inclineShape, StaticGridRotation rotation)
        {
            if (inclineShape == null) { return; }
            inclineShape.InclineSurfaceNormalDirection = new ComposedStaticGridDirection(rotation * inclineShape.InclineSurfaceNormalDirection.Transform);
        }

        private static void RotateBoxShape(GridBoxCollisionShape boxShape, StaticGridRotation rotation)
        {
            if (boxShape == null) { return; }
            boxShape.ResetSize(rotation * boxShape.MinPoint, rotation * boxShape.MaxPoint);
        }

        public void ToOriginRotation()
        {
            Rotate(currentRotation.Inverse);
            currentRotation = StaticGridRotation.Identity;
        }

        //private static void UndoSimpleShape(SimpleGridCollisionShape simpleShape, IDictionary<SimpleGridCollisionShape, SingleShapeInitialStatusData> shapeInitStatus, out Vector3i shapePos)
        //{
        //    Debug.Assert(shapeInitStatus.ContainsKey(simpleShape));
        //    SingleShapeInitialStatusData statusData = shapeInitStatus[simpleShape];
        //    if (simpleShape is GridBoxCollisionShape boxShape)
        //    {
        //        boxShape.ResetSize(statusData.AabbMin, statusData.AabbMax);
        //    }
        //    shapePos = statusData.ShapePosition;
        //}

        private struct SingleShapeInitialStatusData
        {
            public SingleShapeInitialStatusData(Vector3i shapePosition, Vector3i aabbMin, Vector3i aabbMax, ComposedStaticGridDirection inclineDirection)
            {
                ShapePosition = shapePosition;
                AabbMin = aabbMin;
                AabbMax = aabbMax;
                InclineDirection = inclineDirection;
            }

            public Vector3i ShapePosition { get; }
            public Vector3i AabbMin { get; }
            public Vector3i AabbMax { get; }
            public ComposedStaticGridDirection InclineDirection { get; }
        }
    }
}
