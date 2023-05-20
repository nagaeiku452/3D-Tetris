using MainGame.Numeric;
using MainGame.Physics.Blocking;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using MainGame.Physics.Utilities;
using System.Diagnostics;
using _3D_Tetris.Drawing;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisField
    {
        private readonly IThreeDimTetrisWallKickConfiguration<TetrisBodyBase> wallKickConfiguration = new TetraCubeBasicWallKickConfiguration();
        private readonly TetraCubeTetrisWallKickResult<TetrisBodyBase> wallKickResult = new();
        private RotatableTetris currentTetris;
        private readonly static GridGravity oriGravity = new(GameConfigData.TetrisWorldGravityScalar, StaticGridDirection.NegativeUnitZ);
        //private readonly IThreeDimTetrisSet threeDimTetrisSet = new ITetraCubeTetrisSet();

        private readonly List<TetrisBodyBase> walls = new();
        private readonly TFOnlyCollisionResult tFOnlyCollisionResult = new();
        private readonly TetrisSliceExecuter sliceExecuter = new();
        private readonly InstancePrototypeContainer<UnitTetris> unitTetrisContainer = new();

        private Vector3i viewBoxMin;
        private Vector3i viewBoxMax;

        private readonly TetrisBodyBase currentTetrisShadow = new(StaticGridRigidBodyType.Static)
        {
            CollisionFlags = GridCollisionFlags.NoCollision,
            CollisionFilter = GridCollisionFlags.NoCollision,
            IsShadow = true,
            PaintColor = Color.FromArgb(GameConfigData.CurrentTetrisShadowColorA, GameConfigData.CurrentTetrisShadowColorR, GameConfigData.CurrentTetrisShadowColorG, GameConfigData.CurrentTetrisShadowColorB)
        };
        private readonly MinLengthShapeSweepResult shapeSweepResult = new();

        public StaticGridDynamicWorld<TetrisBodyBase> TetrisWorld { get; private set; }
        public Vector3i ViewBoxMin => viewBoxMin;
        public Vector3i ViewBoxMax => viewBoxMax;

        public EventHandler<SliceClearedEventArgs> SliceCleared
        {
            get => sliceExecuter.SliceCleared;
            set => sliceExecuter.SliceCleared = value;
        }

        public EventHandler<CurrentTetrisHardDropEventArgs> CurrentTetrisHardDropped;
        public EventHandler<EventArgs> CurrentTetrisSingleFall;
        public EventHandler<EventArgs> CurrentTetrisFallToStack;

        public bool IsCurrentTetrisAboutToFallToStack
        {
            get
            {
                shapeSweepResult.ResetResult(currentTetris, 1);
                TetrisWorld.ShapeSweepTest(currentTetris.GridCollisionShape, currentTetris.WorldTransform, StaticGridDirection.NegativeUnitZ, 1, shapeSweepResult);
                return shapeSweepResult.MaximumPossibleTransform == 0;
            }
        }

        public void InitNewTetrisField(Vector3i viewBoxMin, Vector3i viewBoxMax)
        {
            TetrisWorld = new StaticGridDynamicWorld<TetrisBodyBase>(new StaticGridCollisionDispatcher(new DefaultStaticGridCollisionAlgorithmConfiguration()), new BlockingPhysicsConfiguration<TetrisBodyBase>())
            {
                Gravity = oriGravity
            };
            TetrisWorld.ClearWorld();

            AddWorldBoundary(TetrisWorld, viewBoxMin, viewBoxMax);
            this.viewBoxMin = viewBoxMin;
            this.viewBoxMax = viewBoxMax;
        }

        public void AddNewCurrentTetris(RotatableTetris newTetris)
        {
            TetrisWorld.RemoveRigidBody(currentTetris);
            currentTetris = newTetris;
            TetrisWorld.AddRigidBody(currentTetris);
            ModifyTetrisShadow(currentTetris, TetrisWorld);
        }

        /// <summary>
        /// rotate current tetris
        /// </summary>
        /// <param name="rotateDir">rotate dir</param>
        /// <returns>true if rotation success.</returns>
        public bool RotateCurrentTetris(StaticGridDirection rotateDir)
        {
            wallKickResult.ResetResult();
            wallKickConfiguration.WallKickTest(currentTetris, TetrisWorld, rotateDir, wallKickResult);
            if (wallKickResult.CanRotate)
            {
                RemoveTetrisShadow(TetrisWorld);
                currentTetris.Rotate(rotateDir);
                currentTetris.WorldTransform += wallKickResult.TransformOffset;
                foreach (TetrisBodyBase item in walls)
                {
                    tFOnlyCollisionResult.ResetResult();
                    TetrisWorld.CollisionTest(item, Vector3i.Zero, tFOnlyCollisionResult);
                    Debug.Assert(!tFOnlyCollisionResult.HasCollision);
                }
                ModifyTetrisShadow(currentTetris, TetrisWorld);
            }
            return wallKickResult.CanRotate;
        }

        public void MoveCurrentTetris(StaticGridDirection moveDirection)
        {
            if (moveDirection != StaticGridDirection.NoDirection)
            {
                MoveCurrentTetris(moveDirection.ToTransform());
            }
        }
        public bool MoveCurrentTetris(Vector3i teleportationDisplacement)
        {
            RemoveTetrisShadow(TetrisWorld);

            Vector3i previousTransform = currentTetris.WorldTransform;
            TetrisWorld.SingleTransformationSimulation(currentTetris, teleportationDisplacement, StaticGridDirection.NoDirection);

            ModifyTetrisShadow(currentTetris, TetrisWorld);

            return previousTransform == currentTetris.WorldTransform;
        }

        public bool FallCurrentTetris()
        {
            Vector3i curTetrisPos = currentTetris.WorldTransform;
            TetrisWorld.SingleTransformationSimulation(currentTetris, -Vector3i.UnitZ, StaticGridDirection.NoDirection);

            CurrentTetrisSingleFall?.Invoke(this, EventArgs.Empty);

            bool b = curTetrisPos == currentTetris.WorldTransform;
            if (b)
            {
                CurrentTetrisFallToTetrisStack(TetrisWorld, currentTetris, unitTetrisContainer);
            }
            return !b;
        }

        public void CurrentTetrisHardDrop()
        {
            Vector3i previousCurrentTetrisPos = currentTetris.WorldTransform;
            //drop the tetris
            currentTetris.AffectByGravity = true;
            TetrisWorld.SingleTransformationSimulation(currentTetris, Vector3i.Zero, StaticGridDirection.NoDirection);

            CurrentTetrisHardDropped?.Invoke(this, new CurrentTetrisHardDropEventArgs(Math.Abs(previousCurrentTetrisPos.Z - currentTetris.WorldTransform.Z)));

            CurrentTetrisFallToTetrisStack(TetrisWorld, currentTetris, unitTetrisContainer);
        }

        private void CurrentTetrisFallToTetrisStack(StaticGridDynamicWorld<TetrisBodyBase> tetrisWorld, RotatableTetris currentTetris, InstancePrototypeContainer<UnitTetris> unitTetrisContainer)
        {
            RemoveTetrisShadow(tetrisWorld);
            DestroyCurrentTetris(currentTetris, tetrisWorld, unitTetrisContainer);
            sliceExecuter.SliceExecution(currentTetris.MinPoint.Z, currentTetris.MaxPoint.Z, currentTetris.ShapeSize, viewBoxMin, viewBoxMax, tetrisWorld, unitTetrisContainer);
            CurrentTetrisFallToStack?.Invoke(this, EventArgs.Empty);
        }

        private void RemoveTetrisShadow(StaticGridDynamicWorld<TetrisBodyBase> tetrisWorld)
        {
            tetrisWorld.RemoveRigidBody(currentTetrisShadow);
        }

        private void ModifyTetrisShadow(RotatableTetris currentTetris, StaticGridDynamicWorld<TetrisBodyBase> tetrisWorld)
        {
            int sweepLength = currentTetris.MinPoint.Z - viewBoxMin.Z;
            shapeSweepResult.ResetResult(currentTetris, sweepLength);
            tetrisWorld.ShapeSweepTest(currentTetris.GridCollisionShape, currentTetris.WorldTransform, StaticGridDirection.NegativeUnitZ, sweepLength, shapeSweepResult);
            currentTetrisShadow.GridCollisionShape = currentTetris.GridCollisionShape;

            (float, float, float) HSL = RGBtoHSLConverter.RGBToHSL(currentTetris.PaintColor.R / 255f, currentTetris.PaintColor.G / 255f, currentTetris.PaintColor.B / 255f);
            (float, float, float) newRGB = RGBtoHSLConverter.HSLToRGB(HSL.Item1, HSL.Item2 * 0.4f, HSL.Item3 * 0.2f);
            currentTetrisShadow.PaintColor = Color.FromArgb(127, (int)(newRGB.Item1 * 255), (int)(newRGB.Item2 * 255), (int)(newRGB.Item3 * 255));

            currentTetrisShadow.WorldTransform = currentTetris.WorldTransform - shapeSweepResult.MaximumPossibleTransform * Vector3i.UnitZ;
            tetrisWorld.AddRigidBody(currentTetrisShadow);
        }

        //public void UndoCurrentTetrisToOriginRotation()
        //{
        //    currentTetris.ToOriginRotation();
        //}

        private void AddWorldBoundary(StaticGridDynamicWorld<TetrisBodyBase> world, Vector3i viewBoxMin, Vector3i viewBoxMax)
        {
            Vector3i v = Vector3i.One * 2;
            TetrisBodyBase wall_1 = new(StaticGridRigidBodyType.Static)
            {
                WorldTransform = viewBoxMin + new Vector3i(-1, -1, -1),
                GridCollisionShape = new GridBoxCollisionShape(-v, new Vector3i(viewBoxMax.X - viewBoxMin.X + 2, 0, viewBoxMax.Z - viewBoxMin.Z + GameConfigData.SideWallHeightOffset))
            };
            world.AddRigidBody(wall_1);
            TetrisBodyBase wall_2 = new(StaticGridRigidBodyType.Static)
            {
                WorldTransform = viewBoxMin + new Vector3i(-1, -1, -1),
                GridCollisionShape = new GridBoxCollisionShape(-v, new Vector3i(0, viewBoxMax.Y - viewBoxMin.Y + 2, viewBoxMax.Z - viewBoxMin.Z + GameConfigData.SideWallHeightOffset))
            };
            world.AddRigidBody(wall_2);
            TetrisBodyBase wall_3 = new(StaticGridRigidBodyType.Static)
            {
                WorldTransform = wall_1.WorldTransform + new Vector3i(0, viewBoxMax.Y - viewBoxMin.Y + 2, 0) + v,
                GridCollisionShape = wall_1.GridCollisionShape
            };
            world.AddRigidBody(wall_3);
            TetrisBodyBase wall_4 = new(StaticGridRigidBodyType.Static)
            {
                WorldTransform = wall_2.WorldTransform + new Vector3i(viewBoxMax.X - viewBoxMin.X + 2, 0, 0) + v,
                GridCollisionShape = wall_2.GridCollisionShape
            };
            world.AddRigidBody(wall_4);

            TetrisBodyBase ground = new(StaticGridRigidBodyType.Static)
            {
                WorldTransform = viewBoxMin + new Vector3i(-1, -1, -1),
                GridCollisionShape = new GridBoxCollisionShape(-v, new Vector3i(viewBoxMax.X - viewBoxMin.X + 2, viewBoxMax.Y - viewBoxMin.Y + 2, 0))
            };
            world.AddRigidBody(ground);
            walls.Add(wall_1);
            walls.Add(wall_2);
            walls.Add(wall_3);
            walls.Add(wall_4);
        }

        //private static void GenerateNewCurrentTetris(IThreeDimTetrisSet threeDimTetrisSet, RotatableTetris currentTetris, ref (GridCollisionShape, Color) nextTetrisData, StaticGridDynamicWorld<TetrisBodyBase> world)
        //{
        //    currentTetris.ToOriginRotation();
        //    currentTetris.GridCollisionShape = nextTetrisData.Item1;
        //    currentTetris.PaintColor = nextTetrisData.Item2;
        //    nextTetrisData = threeDimTetrisSet.NextTetrisData;
        //    //initial
        //    if (currentTetris.GridCollisionShape == null)
        //    {
        //        currentTetris.GridCollisionShape = nextTetrisData.Item1;
        //        currentTetris.PaintColor = nextTetrisData.Item2;
        //        nextTetrisData = threeDimTetrisSet.NextTetrisData;
        //    }
        //    currentTetris.AffectByGravity = false;
        //    currentTetris.WorldTransform = new Vector3i(GlobalPresetConst.TetraCubeTetrisInitPosX, GlobalPresetConst.TetraCubeTetrisInitPosY, GlobalPresetConst.TetraCubeTetrisInitPosZ);
        //    currentTetris.ForceSetActivationState(GridBodyActivationState.DisableDeactivation);
        //    world.AddRigidBody(currentTetris);
        //}

        private static void DestroyCurrentTetris(RotatableTetris currentTetris, StaticGridDynamicWorld<TetrisBodyBase> world, InstancePrototypeContainer<UnitTetris> unitTetrisContainer)
        {
            world.RemoveRigidBody(currentTetris);
            foreach (Vector3i v in currentTetris.UnitBoxDecomposition)
            {
                UnitTetris unit = unitTetrisContainer.RentPrototypeInstance();
                unit.WorldTransform = v;
                unit.PaintColor = currentTetris.PaintColor;
                world.AddRigidBody(unit);
            }
        }

        private class MinLengthShapeSweepResult : IShapeSweepResult<TetrisBodyBase>
        {
            public int MaximumPossibleTransform { get; private set; }
            private TetrisBodyBase me;
            public void AddSingleResult(TetrisBodyBase otherObject, int maximumPossibleTransform)
            {
                if (otherObject != me&& MaximumPossibleTransform> maximumPossibleTransform)
                {
                    MaximumPossibleTransform = maximumPossibleTransform;
                }
            }

            public void ResetResult(TetrisBodyBase me, int maxTransform)
            {
                this.me = me;
                MaximumPossibleTransform = maxTransform;
            }
        }
    }
}
