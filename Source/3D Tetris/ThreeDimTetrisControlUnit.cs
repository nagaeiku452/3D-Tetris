using MainGame.Numeric;
using MainGame.Physics.Blocking;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3D_Tetris
{
    internal class ThreeDimTetrisControlUnit : WinFormControlUnit, IDisposable
    {
        private MainWindow mainWindow;
        private Image frontImage;
        private Image backImage;
        private WorldViewPainter worldViewPainter;

        private SingleTetrisPainter nextSingleTetrisPainter;
        private SingleTetrisPainter heldSingleTetrisPainter;

        private readonly Point[] nextTetrisPos = new Point[3];

        //private readonly MultiBoxShapeReplicator holdShapeReplicator = new();
        //private readonly MultiBoxShapeReplicator tempShapeReplicator = new();
        //private Color holdShapeColor = Color.AliceBlue;
        private readonly TetrisMorphling heldTetris = new();
        private bool IsHeldTetris = false;


        //TetrisBodyBase testTetris;
        private readonly ThreeDimTetrisField TetrisField = new();
        private readonly ThreeDimTetrisScoreCounter scoreCounter = new();
        private readonly ThreeDimTetrisSliceClearedCounter sliceCounter = new();
        private readonly ThreeDimTetrisLevelCounter levelCounter = new();
        private readonly ThreeDimTetrisFallTimer fallTimer = new();
        private readonly ThreeDimTetrisTotalPlayTimeCounter playTimeCounter = new();
        private readonly NextTetrisQueue generator = new()
        {
            QueueSize = 3,
        };
        private readonly ThreeDimTetrisUIManager uIManager = new();


        //int tetrisFallThreshold = GlobalPresetConst.TetrisFallInterval;
        private CameraViewAngle curViewAngle = CameraViewAngle.firstQuadrant;
        private readonly TetrisMorphling currentTetris = new();

        private readonly TotalTetrisDropCounter dropCounter = new();
        //int tetrisCount = 0;

        private static readonly Color BackGroundColor = Color.FromArgb(GameConfigData.BackgroundColorR, GameConfigData.BackgroundColorG, GameConfigData.BackgroundColorB);

        public override void Dispose()
        {
            frontImage.Dispose();
            backImage.Dispose();
            worldViewPainter.Dispose();
            TetrisField.SliceCleared -= OnSliceCleared;
            TetrisField.CurrentTetrisHardDropped -= OnCurrentTetrisHardDropped;
            TetrisField.CurrentTetrisFallToStack -= OnCurrentTetrisFallToStack;
            fallTimer.TetrisFall -= OnCurrentTetrisFallIntervalTimeUp;
        }

        public override void InitControlUnit(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            Vector3i viewBoxMax = new(GameConfigData.WorldLengthX - 1, GameConfigData.WorldLengthY - 1, GameConfigData.WorldLengthZ - 1);
            TetrisField.InitNewTetrisField(Vector3i.Zero, viewBoxMax);
            GroundPainter gPainter = new();

            gPainter.ConfigureProjection(new Point(GameConfigData.WorldTetrisUnitZSurfaceWidth / 2, GameConfigData.WorldTetrisUnitZSurfaceHeight / 2), new Point(-GameConfigData.WorldTetrisUnitZSurfaceWidth / 2, GameConfigData.WorldTetrisUnitZSurfaceHeight / 2), viewBoxMax.X + 1, viewBoxMax.Y + 1);
            BWBoxSprite boxSprite1 = new();
            boxSprite1.GenerateImage(GameConfigData.WorldTetrisUnitBoxHeight, GameConfigData.WorldTetrisUnitZSurfaceHeight, GameConfigData.WorldTetrisUnitZSurfaceWidth, GameConfigData.XSurfaceBrightness, GameConfigData.YSurfaceBrightness, GameConfigData.ZSurfaceBrightness);
            worldViewPainter = new WorldViewPainter(Vector3i.Zero, viewBoxMax, boxSprite1, gPainter);

            BWBoxSprite boxSprite2 = new();
            boxSprite2.GenerateImage(GameConfigData.TetrisModelUnitBoxHeight, GameConfigData.TetrisModelUnitZSurfaceHeight, GameConfigData.TetrisModelUnitZSurfaceWidth, GameConfigData.XSurfaceBrightness, GameConfigData.YSurfaceBrightness, GameConfigData.ZSurfaceBrightness);
            nextSingleTetrisPainter = new SingleTetrisPainter(boxSprite2);

            BWBoxSprite boxSprite3 = new();
            boxSprite3.GenerateImage(GameConfigData.HeldTetrisUnitBoxHeight, GameConfigData.HeldTetrisUnitZSurfaceHeight, GameConfigData.HeldTetrisUnitZSurfaceWidth, GameConfigData.XSurfaceBrightness, GameConfigData.YSurfaceBrightness, GameConfigData.ZSurfaceBrightness);
            heldSingleTetrisPainter = new SingleTetrisPainter(boxSprite3);

            backImage = new Bitmap(mainWindow.GameWindow.Size.Width, mainWindow.GameWindow.Size.Height);
            frontImage = new Bitmap(mainWindow.GameWindow.Size.Width, mainWindow.GameWindow.Size.Height);

            nextTetrisPos[0] = mainWindow.NextTetris1.Location;
            nextTetrisPos[1] = mainWindow.NextTetris2.Location;
            nextTetrisPos[2] = mainWindow.NextTetris3.Location;

            uIManager.Initialize(mainWindow);

            scoreCounter.InitNewScore();
            TetrisField.SliceCleared += OnSliceCleared;
            TetrisField.CurrentTetrisHardDropped += OnCurrentTetrisHardDropped;
            TetrisField.CurrentTetrisFallToStack += OnCurrentTetrisFallToStack;

            levelCounter.InitNewCounter();

            fallTimer.ResetFallInterval(levelCounter.CurrentLevel);
            fallTimer.TetrisFall += OnCurrentTetrisFallIntervalTimeUp;

            playTimeCounter.InitTotalPlayTime();

            GeneratrNewTetris(currentTetris, TetrisField, curViewAngle, generator, false);
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mainWindow.Close();
            }
            else if (e.KeyCode == Keys.Up)
            {
                TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, curViewAngle) * -Vector3i.UnitY);
            }
            else if (e.KeyCode == Keys.Down)
            {
                TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, curViewAngle) * Vector3i.UnitY);
            }
            else if (e.KeyCode == Keys.Left)
            {
                TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, curViewAngle) * -Vector3i.UnitX);
            }
            else if (e.KeyCode == Keys.Right)
            {
                TetrisField.MoveCurrentTetris(GetRotationByViewAngle(CameraViewAngle.firstQuadrant, curViewAngle) * Vector3i.UnitX);
            }
            else if (e.KeyCode == Keys.Space)
            {
                TetrisField.CurrentTetrisHardDrop();
            }
            else if (e.KeyCode == Keys.Z)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitX, curViewAngle));
            }
            else if (e.KeyCode == Keys.A)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitX, curViewAngle));
            }
            else if (e.KeyCode == Keys.X)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitY, curViewAngle));
            }
            else if (e.KeyCode == Keys.S)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitY, curViewAngle));
            }
            else if (e.KeyCode == Keys.C)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.PositiveUnitZ, curViewAngle));
            }
            else if (e.KeyCode == Keys.D)
            {
                TetrisField.RotateCurrentTetris(AdjustRotationByCameraViewAngle(StaticGridDirection.NegativeUnitZ, curViewAngle));
            }
            else if (e.KeyCode == Keys.Q)
            {
                curViewAngle = curViewAngle.CounterClockWisedShift();
            }
            else if (e.KeyCode == Keys.E)
            {
                curViewAngle = curViewAngle.ClockWisedShift();
            }
            else if (e.KeyCode == Keys.NumPad0)
            {
                fallTimer.SoftDropEnabled = true;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                if (!IsHeldTetris)
                {
                    HoldTetris();
                }
            }
        }

        private void HoldTetris()
        {
            int currentTetrisNum = currentTetris.TetrisNum;
            int heldTetrisNum = heldTetris.TetrisNum;
            heldTetris.CloneTetrisData(generator.GetTetrisData(currentTetrisNum), currentTetrisNum);


            if (heldTetrisNum == -1)
            {
                GeneratrNewTetris(currentTetris, TetrisField, curViewAngle, generator, true);
            }
            else
            {
                currentTetris.CloneTetrisData(generator.GetTetrisData(heldTetrisNum), heldTetrisNum);
                AddNewTetris(currentTetris, TetrisField, curViewAngle, true);
            }
        }

        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0)
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

        public override void OnRenderLoopEvent(object sender, EventArgs e)
        {
            base.OnRenderLoopEvent(sender, e);

            //clear background
            Graphics g = Graphics.FromImage(backImage);
            g.Clear(BackGroundColor);

            //paint world
            worldViewPainter.PaintWorld(TetrisField.TetrisWorld, backImage, mainWindow.WorldOrigin.Location, curViewAngle);

            //paint next tetrises
            int i = 0;
            foreach ((GridCollisionShape, Color) item in generator.EnumerateNextTetrisData)
            {
                if (i < 3)
                {
                    PaintSingleTetris(nextSingleTetrisPainter, backImage, nextTetrisPos[i], item.Item1, item.Item2, false);
                }
                i++;
            }

            //paint hold tetris

            PaintSingleTetris(heldSingleTetrisPainter, backImage, mainWindow.HoldTetris.Location, heldTetris.Instance.GridCollisionShape, heldTetris.Instance.PaintColor, IsHeldTetris);

            //switch
            Image temp = frontImage;
            frontImage = backImage;
            backImage = temp;
            mainWindow.GameWindow.BackgroundImage = frontImage;

            ShowUIData();

            mainWindow.Log.Text = $"{((dropCounter.TotalTetrisDropped) / playTimeCounter.TotalPlayTime.TotalSeconds):F2}";
        }

        private static void PaintSingleTetris(SingleTetrisPainter bigSingleTetrisPainter, Image backImage, Point anchor, GridCollisionShape shape, Color color, bool IsHeld)
        {
            bigSingleTetrisPainter.PaintSingleTetris(backImage, anchor, shape, color, IsHeld);
        }

        public override void OnGameLoopEvent(object sender, EventArgs e)
        {
            base.OnGameLoopEvent(sender, e);
            fallTimer.Tick();
            playTimeCounter.AddPlayTime(GameConfigData.GameLoopInterval);
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
            GeneratrNewTetris(currentTetris, TetrisField, curViewAngle, generator, false);

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
            return (GetRotationByViewAngle(startViewAngle) ^ (-1)) * GetRotationByViewAngle(endViewAngle);
        }

        private void OnCurrentTetrisFallIntervalTimeUp(object sender, FallEventArgs fe)
        {
            TetrisField.FallCurrentTetris();
            if (fe.IsSoftDrop)
            {
                scoreCounter.OnCurrentTetrisSoftDropped(levelCounter.CurrentLevel);
            }
        }

        private void ShowUIData()
        {
            uIManager.ShowScore(scoreCounter.CurrentScore);
            uIManager.ShowSliceCleared(sliceCounter.TotalSliceCleared);
            uIManager.ShowLevel(levelCounter.CurrentLevel);
            uIManager.ShowPlayTime(playTimeCounter.TotalPlayTime);
            uIManager.ShowTetrisDropped(dropCounter.TotalTetrisDropped);
        }

        private void AdjustCurrentLevel(int totalSliceCleared)
        {
            levelCounter.AdjustCurrentLevel(totalSliceCleared);
        }
    }
}
