using _3D_Tetris.Drawing;
using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
//using System.Drawing;

namespace _3D_Tetris
{
    public class ThreeDimTetrisControlUnit : IDisposable
    {

        private readonly TetrisMorphling currentTetris = new();
        private readonly TotalTetrisDropCounter dropCounter = new();
        private readonly TetrisMorphling heldTetris = new();
        private readonly ThreeDimTetrisField TetrisField = new();
        private readonly ThreeDimTetrisScoreCounter scoreCounter = new();
        private readonly ThreeDimTetrisSliceClearedCounter sliceCounter = new();
        private readonly ThreeDimTetrisLevelCounter levelCounter = new();
        private readonly ThreeDimTetrisFallTimer fallTimer = new();
        private readonly NextTetrisQueue generator = new()
        {
            QueueSize = 3,
        };

        public IEnumerable<(GridCollisionShape, Color)> NextTetrisData => generator.EnumerateNextTetrisData;

        public event EventHandler<EventArgs> WorldUpdated;

        public int CurrentScore => scoreCounter.CurrentScore;
        public int CurrentClearedSlices => sliceCounter.TotalSliceCleared;
        public int CurrentLevel => levelCounter.CurrentLevel;
        public int TotalTetrisDropped => dropCounter.TotalTetrisDropped;

        public TetrisBodyBase CurrentTetris => currentTetris.Instance;
        public TetrisBodyBase HeldTetris => heldTetris.Instance;

        public StaticGridDynamicWorld<TetrisBodyBase> TetrisWorld => TetrisField.TetrisWorld;

        public CameraViewAngle CurViewAngle { get; private set; } = CameraViewAngle.firstQuadrant;
        public bool IsHeldTetris { get; private set; }
        private TetrisGameInstruction pendingInstruction = TetrisGameInstruction.None;
        //int tetrisCount = 0;

        public static readonly Color BackGroundColor = Color.FromRgb(GameConfigData.BackgroundColorR, GameConfigData.BackgroundColorG, GameConfigData.BackgroundColorB);

        public ThreeDimTetrisControlUnit(Vector3i viewBoxMax)
        {
            TetrisField.InitNewTetrisField(Vector3i.Zero, viewBoxMax);

            scoreCounter.InitNewScore();
            TetrisField.SliceCleared += OnSliceCleared;
            TetrisField.CurrentTetrisHardDropped += OnCurrentTetrisHardDropped;
            TetrisField.CurrentTetrisFallToStack += OnCurrentTetrisFallToStack;

            levelCounter.InitNewCounter();

            fallTimer.ResetFallInterval(levelCounter.CurrentLevel);
            fallTimer.TetrisFall += OnCurrentTetrisFallIntervalTimeUp;

            GeneratrNewTetris(currentTetris, TetrisField, CurViewAngle, generator, false);
        }

        void IDisposable.Dispose()
        {
            TetrisField.SliceCleared -= OnSliceCleared;
            TetrisField.CurrentTetrisHardDropped -= OnCurrentTetrisHardDropped;
            TetrisField.CurrentTetrisFallToStack -= OnCurrentTetrisFallToStack;
            fallTimer.TetrisFall -= OnCurrentTetrisFallIntervalTimeUp;
            GC.SuppressFinalize(this);
        }

        public void OnKeyDown(TetrisGameInstruction newKeyInstruction)
        {
            if (pendingInstruction == TetrisGameInstruction.None)
            {
                pendingInstruction = newKeyInstruction;
            }
        }

        private void ExecuteGameInstruction(TetrisGameInstruction newKeyInstruction)
        {
            switch (newKeyInstruction)
            {
                case TetrisGameInstruction.None:
                    return;
                case TetrisGameInstruction.MoveUpLeft:
                    TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, CurViewAngle) * -Vector3i.UnitX);
                    break;
                case TetrisGameInstruction.MoveUpRight:
                    TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, CurViewAngle) * -Vector3i.UnitY);
                    break;
                case TetrisGameInstruction.MoveDownLeft:
                    TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, CurViewAngle) * Vector3i.UnitY);
                    break;
                case TetrisGameInstruction.MoveDownRight:
                    TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, CurViewAngle) * Vector3i.UnitX);
                    break;
                case TetrisGameInstruction.PosXRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitX, CurViewAngle));
                    break;
                case TetrisGameInstruction.PosYRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitY, CurViewAngle));
                    break;
                case TetrisGameInstruction.PosZRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitZ, CurViewAngle));
                    break;
                case TetrisGameInstruction.NegXRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitX, CurViewAngle));
                    break;
                case TetrisGameInstruction.NegYRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitY, CurViewAngle));
                    break;
                case TetrisGameInstruction.NegZRotation:
                    TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitZ, CurViewAngle));
                    break;
                case TetrisGameInstruction.ClockWiseCameraRotate:
                    CurViewAngle = CurViewAngle.ClockWisedShift();
                    break;
                case TetrisGameInstruction.CounterClockWiseCameraRotate:
                    CurViewAngle = CurViewAngle.CounterClockWisedShift();
                    break;
                case TetrisGameInstruction.SoftDrop:
                    fallTimer.SoftDropEnabled = true;
                    return;
                case TetrisGameInstruction.HardDrop:
                    TetrisField.CurrentTetrisHardDrop();
                    break;
                case TetrisGameInstruction.Hold:
                    if (!IsHeldTetris)
                    {
                        HoldTetris();
                    }
                    break;
                default:
                    return;
            }
            if (newKeyInstruction != TetrisGameInstruction.SoftDrop || newKeyInstruction != TetrisGameInstruction.None)
            {
                WorldUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HoldTetris()
        {
            int currentTetrisNum = currentTetris.TetrisNum;
            int heldTetrisNum = heldTetris.TetrisNum;
            heldTetris.CloneTetrisData(generator.GetTetrisData(currentTetrisNum), currentTetrisNum);


            if (heldTetrisNum == -1)
            {
                GeneratrNewTetris(currentTetris, TetrisField, CurViewAngle, generator, true);
            }
            else
            {
                currentTetris.CloneTetrisData(generator.GetTetrisData(heldTetrisNum), heldTetrisNum);
                AddNewTetris(currentTetris, TetrisField, CurViewAngle, true);
            }
        }

        public void OnKeyUp(TetrisGameInstruction newKeyInstruction)
        {
            if (newKeyInstruction == TetrisGameInstruction.SoftDrop)
            {
                fallTimer.SoftDropEnabled = false;
            }
        }

        private static StaticGridDirection AdjustRotationByCameraViewAngle(StaticGridDirection oriDir, CameraViewAngle curViewAngle)
        {
            if (oriDir == StaticGridDirection.NoDirection || oriDir == StaticGridDirection.PositiveUnitZ || oriDir == StaticGridDirection.NegativeUnitZ)
            {
                return oriDir;
            }

            return curViewAngle switch
            {
                CameraViewAngle.firstQuadrant => oriDir,
                CameraViewAngle.secondQuadrant => oriDir.CrossProductWith(StaticGridDirection.PositiveUnitZ),
                CameraViewAngle.thirdQuadrant => oriDir.ToOppositeDirection(),
                CameraViewAngle.fourthQuadrant => oriDir.CrossProductWith(StaticGridDirection.NegativeUnitZ),
                _ => oriDir,
            };
        }

        //private static StaticGridDirection AdjustTranslationByCameraViewAngle(StaticGridDirection oriDir, CameraViewAngle curViewAngle)
        //{
        //    if (oriDir == StaticGridDirection.NoDirection || oriDir == StaticGridDirection.PositiveUnitZ || oriDir == StaticGridDirection.NegativeUnitZ)
        //    {
        //        return oriDir;
        //    }

        //    return curViewAngle switch
        //    {
        //        CameraViewAngle.firstQuadrant => oriDir,
        //        CameraViewAngle.secondQuadrant => oriDir.CrossProductWith(StaticGridDirection.NegativeUnitZ),
        //        CameraViewAngle.thirdQuadrant => oriDir.ToOppositeDirection(),
        //        CameraViewAngle.fourthQuadrant => oriDir.CrossProductWith(StaticGridDirection.PositiveUnitZ),
        //        _ => oriDir,
        //    };
        //}

        //public void OnRenderLoopEvent()
        //{

        //    //clear background
        //    Graphics g = Graphics.FromImage(backImage);
        //    g.Clear(BackGroundColor);

        //    //paint world
        //    worldViewPainter.PaintWorld(TetrisField.TetrisWorld, backImage, mainWindow.WorldOrigin.Location, CurViewAngle);

        //    //paint next tetrises
        //    int i = 0;
        //    foreach ((GridCollisionShape, Color) item in generator.EnumerateNextTetrisData)
        //    {
        //        if (i < 3)
        //        {
        //            PaintSingleTetris(nextSingleTetrisPainter, backImage, nextTetrisPos[i], item.Item1, item.Item2, false);
        //        }
        //        i++;
        //    }

        //    //paint hold tetris

        //    PaintSingleTetris(heldSingleTetrisPainter, backImage, mainWindow.HoldTetris.Location, heldTetris.Instance.GridCollisionShape, heldTetris.Instance.PaintColor, IsHeldTetris1);

        //    //switch
        //    (backImage, frontImage) = (frontImage, backImage);
        //    mainWindow.GameWindow.BackgroundImage = frontImage;

        //    ShowUIData();

        //    mainWindow.Log.Text = $"{dropCounter.TotalTetrisDropped / playTimeCounter.TotalPlayTime.TotalSeconds:F2}";
        //}

        //private static void PaintSingleTetris(SingleTetrisPainter bigSingleTetrisPainter, Image backImage, Point anchor, GridCollisionShape shape, Color color, bool IsHeld)
        //{
        //    bigSingleTetrisPainter.PaintSingleTetris(backImage, anchor, shape, color, IsHeld);
        //}

        public void OnGameLoopEvent()
        {
            ExecuteGameInstruction(pendingInstruction);
            pendingInstruction = TetrisGameInstruction.None;
            fallTimer.Tick();
        }

        //private void PaintWorld(StaticGridDynamicWorld<TetrisBodyBase> world, Image canvas, CameraViewAngle curViewAngle)
        //{
        //}

        //private static void PaintSingleTetris(SingleTetrisPainter painter, StaticGridDynamicWorld<TetrisBodyBase> world, Image canvas, CameraViewAngle curViewAngle, Point anchor, TetrisBodyBase paintTetris)
        //{
        //    world.AddCollisionObject(paintTetris);
        //    painter.PaintWorld(world, canvas, anchor, singleTetrisShader, curViewAngle);
        //    world.RemoveCollisionObject(paintTetris);
        //}

        private void OnSliceCleared(object sender, SliceClearedEventArgs e)
        {
            scoreCounter.OnSliceClear(e.SliceCleared, levelCounter.CurrentLevel);
            sliceCounter.OnSliceClear(e.SliceCleared);

            AdjustCurrentLevel(sliceCounter.TotalSliceCleared);
        }

        private void OnCurrentTetrisHardDropped(object sender, CurrentTetrisHardDropEventArgs e)
        {
            fallTimer.ResetFallInterval(levelCounter.CurrentLevel);
            scoreCounter.OnCurrentTetrisHardDropped(e.DroppedLength, levelCounter.CurrentLevel);
        }
        private void OnCurrentTetrisFallToStack(object sender, EventArgs _)
        {
            fallTimer.ResetFallInterval(levelCounter.CurrentLevel);
            GeneratrNewTetris(currentTetris, TetrisField, CurViewAngle, generator, false);

            dropCounter.Dropped();
        }

        private void GeneratrNewTetris(TetrisMorphling currentTetris, ThreeDimTetrisField tetrisField, CameraViewAngle curViewAngle, NextTetrisQueue generator, bool isHoldTetris)
        {
            currentTetris.Instance.ToOriginRotation();
            int num = generator.NextTetrisData;
            (GridCollisionShape, Color) nextTetrisData = generator.GetTetrisData(num);
            currentTetris.CloneTetrisData(nextTetrisData, num);
            AddNewTetris(currentTetris, tetrisField, curViewAngle, isHoldTetris);
        }

        private void AddNewTetris(TetrisMorphling currentTetris, ThreeDimTetrisField tetrisField, CameraViewAngle curViewAngle, bool isHoldTetris)
        {
            StaticGridRotation viewRotation = GetRotationByViewAngle(CameraViewAngle.firstQuadrant, curViewAngle);
            currentTetris.Instance.RotatableShape.Rotate(viewRotation);
            Vector3i v = new(GameConfigData.CurrentTetrisInitPosX, GameConfigData.CurrentTetrisInitPosY, tetrisField.ViewBoxMax.Z);
            Vector3i v2 = viewRotation * (2 * v - tetrisField.ViewBoxMax) + tetrisField.ViewBoxMax + tetrisField.ViewBoxMin;
            currentTetris.Instance.WorldTransform = new Vector3i(v2.X / 2, v2.Y / 2, v2.Z / 2);
            currentTetris.Instance.AffectByGravity = false;
            currentTetris.Instance.ForceSetActivationState(GridBodyActivationState.DisableDeactivation);
            tetrisField.AddNewCurrentTetris(currentTetris.Instance);
            IsHeldTetris = isHoldTetris;
        }

        private static StaticGridRotation GetRotationByViewAngle(CameraViewAngle viewAngle)
        {
            return viewAngle switch
            {
                CameraViewAngle.firstQuadrant => StaticGridRotation.Identity,
                CameraViewAngle.secondQuadrant => StaticGridRotation.PositiveZRotation,
                CameraViewAngle.thirdQuadrant => StaticGridRotation.PositiveZRotation ^ 2,
                CameraViewAngle.fourthQuadrant => StaticGridRotation.NegativeZRotation,
                _ => StaticGridRotation.Identity,
            };
        }

        private static StaticGridRotation GetRotationByViewAngle(CameraViewAngle startViewAngle, CameraViewAngle endViewAngle)
        {
            return (GetRotationByViewAngle(startViewAngle) ^ -1) * GetRotationByViewAngle(endViewAngle);
        }

        private void OnCurrentTetrisFallIntervalTimeUp(object sender, FallEventArgs fe)
        {
            TetrisField.FallCurrentTetris();
            WorldUpdated?.Invoke(this, EventArgs.Empty);
            if (fe.IsSoftDrop)
            {
                scoreCounter.OnCurrentTetrisSoftDropped(levelCounter.CurrentLevel);
            }
        }

        private void AdjustCurrentLevel(int totalSliceCleared)
        {
            levelCounter.AdjustCurrentLevel(totalSliceCleared);
        }
    }
}
