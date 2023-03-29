using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using MainGame.Physics.Utilities;
using System;
using System.Collections.Generic;

namespace _3D_Tetris
{
    internal class TetrisSliceExecuter
    {
        private readonly List<int> filledSlicesHeight = new();
        private readonly GridBoxCollisionShape destroySliceSweepShape = new(new(), new());
        private readonly AllShapeCastResult<TetrisBodyBase> allShapeCastResult = new();
        private readonly Dictionary<TetrisBodyBase, Vector3i> sliceMovementBuffer = new();
        private readonly SliceClearedEventArgs sliceClearedEventArg = new();

        private readonly GridBoxCollisionShape sliceSweepShape = new(new(), new());
        private readonly TFOnlyShapeCastResult<TetrisBodyBase> tFOnlyShapeCastResult = new();

        public EventHandler<SliceClearedEventArgs> SliceCleared;

        public void SliceExecution(int Zmin, int Zmax, int curTetrisSize, Vector3i viewBoxMin, Vector3i viewBoxMax, StaticGridDynamicWorld<TetrisBodyBase> tetrisWorld, InstancePrototypeContainer<UnitTetris> unitTetrisContainer)
        {
            //filled slice detection
            filledSlicesHeight.Clear();
            for (int i = Zmin; i <= Zmax; i++)
            {
                if (CheckFilled(tetrisWorld, new Vector3i(viewBoxMin.X, viewBoxMin.Y, i), new Vector3i(viewBoxMax.X, viewBoxMax.Y, i)))
                {
                    filledSlicesHeight.Add(i);
                }
            }

            //destroy filled slices
            for (int i = 0; i < filledSlicesHeight.Count; i++)
            {
                int lowerFilledSlice = filledSlicesHeight[i];
                int upperFilledSlice = i + 1 == filledSlicesHeight.Count ? viewBoxMax.Z + curTetrisSize : filledSlicesHeight[i + 1];

                //destroy slice
                allShapeCastResult.ResetResult();
                destroySliceSweepShape.ResetSize(new Vector3i(viewBoxMin.X, viewBoxMin.Y, lowerFilledSlice), new Vector3i(viewBoxMax.X, viewBoxMax.Y, lowerFilledSlice));
                tetrisWorld.ShapeCastTest(destroySliceSweepShape, new Vector3i(), allShapeCastResult);

                foreach ((TetrisBodyBase, ShapeWrapper) item in allShapeCastResult.CollisionBodies)
                {
                    tetrisWorld.RemoveRigidBody(item.Item1);
                    unitTetrisContainer.ReturnInstance(item.Item1 as UnitTetris);
                }

                //move lines
                if (upperFilledSlice - lowerFilledSlice > 1)
                {
                    allShapeCastResult.ResetResult();
                    destroySliceSweepShape.ResetSize(new Vector3i(viewBoxMin.X, viewBoxMin.Y, lowerFilledSlice + 1), new Vector3i(viewBoxMax.X, viewBoxMax.Y, upperFilledSlice - 1));
                    tetrisWorld.ShapeCastTest(destroySliceSweepShape, new Vector3i(), allShapeCastResult);

                    sliceMovementBuffer.Clear();
                    foreach ((TetrisBodyBase, ShapeWrapper) item in allShapeCastResult.CollisionBodies)
                    {
                        item.Item1.ForceSetActivationState(GridBodyActivationState.Active);
                        sliceMovementBuffer.Add(item.Item1, -Vector3i.UnitZ * (i + 1));
                    }
                    tetrisWorld.SingleTransformationSimulation(sliceMovementBuffer, StaticGridDirection.NoDirection);
                }
            }
            if (filledSlicesHeight.Count > 0)
            {
                sliceClearedEventArg.SliceCleared = filledSlicesHeight.Count;
                SliceCleared?.Invoke(this, sliceClearedEventArg);
            }
        }
        private bool CheckFilled(StaticGridDynamicWorld<TetrisBodyBase> world, Vector3i min, Vector3i max)
        {
            bool IsFilled = true;
            for (int i = min.X; i <= max.X; i++)
            {
                for (int j = min.Y; j <= max.Y; j++)
                {
                    for (int k = min.Z; k <= max.Z; k++)
                    {
                        tFOnlyShapeCastResult.ResetResult();
                        world.ShapeCastTest(sliceSweepShape, new Vector3i(i, j, k), tFOnlyShapeCastResult);
                        IsFilled &= tFOnlyShapeCastResult.HasCollision;

                        if (!IsFilled) { break; }
                    }
                }
            }

            return IsFilled;
        }
    }
}
