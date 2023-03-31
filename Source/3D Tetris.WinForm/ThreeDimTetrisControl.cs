using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;

namespace _3D_Tetris.WinForm
{
    internal class ThreeDimTetrisControl : WinFormControlUnit
    {
        private bool testFlag = false;

        private Image frontImage;
        private Image backImage;
        private readonly Image worldImage;

        private readonly WorldViewPainter worldViewPainter;

        private readonly SingleTetrisPainter nextSingleTetrisPainter;
        private readonly SingleTetrisPainter heldSingleTetrisPainter;
        private readonly Point[] nextTetrisPos = new Point[3];

        private readonly ThreeDimTetrisControlUnit mainCotrolUnit;
        private readonly TotalPlayTimeCounter totalPlayTimeCounter = new();
        private readonly ThreeDimTetrisUIManager uIManager;
        private readonly FpsCounter fpsCounter = new();
        private readonly ElapsedTimeCounter gameLoopElapsedTimeCounter = new(GameConfigData.GameLoopInterval);
        private readonly ElapsedTimeCounter renderLoopElapsedTimeCounter = new(GameConfigData.RenderInterval);

        public ThreeDimTetrisControl(MainWindow callerForm) : base(callerForm)
        {
            GroundPainter gPainter = new();

            Vector3i viewBoxMax = new(GameConfigData.WorldLengthX - 1, GameConfigData.WorldLengthY - 1, GameConfigData.WorldLengthZ - 1);
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
            worldImage = new Bitmap(mainWindow.GameWindow.Size.Width, mainWindow.GameWindow.Size.Height);

            nextTetrisPos[0] = mainWindow.NextTetris1.Location;
            nextTetrisPos[1] = mainWindow.NextTetris2.Location;
            nextTetrisPos[2] = mainWindow.NextTetris3.Location;

            uIManager = new(mainWindow);
            mainCotrolUnit = new ThreeDimTetrisControlUnit(viewBoxMax);

            gameLoopElapsedTimeCounter.TimerElapsed += GameLoopEvent;
            renderLoopElapsedTimeCounter.TimerElapsed += RenderLoopEvent;
            mainCotrolUnit.WorldUpdated += OnWorldUpdate;

            PaintWorld();
        }

        public override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mainWindow.Close();
            }
            mainCotrolUnit.OnKeyDown(TetrisGameInstructionTranslator.GetInstruction(e.KeyCode));
        }

        public override void OnKeyUp(object sender, KeyEventArgs e)
        {
            mainCotrolUnit.OnKeyUp(TetrisGameInstructionTranslator.GetInstruction(e.KeyCode));
        }

        public override void OnGameLoopEvent()
        {
            gameLoopElapsedTimeCounter.UpdateTimer();
        }

        private void GameLoopEvent(object? sender, TimerElapsedEventArgs te)
        {
            mainCotrolUnit.OnGameLoopEvent();
            totalPlayTimeCounter.AddPlayTime((int)te.TimeElapsed);
        }

        public override void OnRenderLoopEvent()
        {
            base.OnRenderLoopEvent();
            renderLoopElapsedTimeCounter.UpdateTimer();
        }

        private void OnWorldUpdate(object? sender, EventArgs e)
        {
            PaintWorld();
        }

        private void RenderLoopEvent(object? sender, TimerElapsedEventArgs te)
        {
            //clear background
            Graphics g = Graphics.FromImage(backImage);
            g.Clear(GameConfigData.BackgroundColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //paint world
            g.DrawImage(worldImage,Point.Empty);
            //worldViewPainter.PaintWorld(mainCotrolUnit.TetrisWorld, g, mainWindow.WorldOrigin.Location, mainCotrolUnit.CurViewAngle);

            //paint next tetrises
            int i = 0;
            foreach ((GridCollisionShape, Color) item in mainCotrolUnit.NextTetrisData.Select(v => ((GridCollisionShape, Color))v))
            {
                if (i < 3)
                {
                    nextSingleTetrisPainter.PaintSingleTetris(g, nextTetrisPos[i], item.Item1, item.Item2, false);
                }
                i++;
            }

            //paint hold tetris

            heldSingleTetrisPainter.PaintSingleTetris(g, mainWindow.HoldTetris.Location, mainCotrolUnit.HeldTetris.GridCollisionShape, mainCotrolUnit.HeldTetris.PaintColor, mainCotrolUnit.IsHeldTetris);

            //switch
            (backImage, frontImage) = (frontImage, backImage);
            mainWindow.GameWindow.BackgroundImage = frontImage;

            ShowUIData();

            fpsCounter.InvokeEvent();
            mainWindow.Log.Text = $"{mainCotrolUnit.TotalTetrisDropped / totalPlayTimeCounter.TotalPlayTime.TotalSeconds:F2}";
            mainWindow.Log.Text += Environment.NewLine;
            mainWindow.Log.Text += $"{fpsCounter.CurrentFps:F2}" + (testFlag ? "a" : string.Empty);
            testFlag = !testFlag;
        }

        private void PaintWorld()
        {
            Graphics g = Graphics.FromImage(worldImage);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            worldViewPainter.PaintWorld(mainCotrolUnit.TetrisWorld, g, mainWindow.WorldOrigin.Location, mainCotrolUnit.CurViewAngle);
        }

        public override void Dispose()
        {

        }

        private void ShowUIData()
        {
            uIManager.ShowScore(mainCotrolUnit.CurrentScore);
            uIManager.ShowSliceCleared(mainCotrolUnit.CurrentClearedSlices);
            uIManager.ShowLevel(mainCotrolUnit.CurrentLevel);
            uIManager.ShowPlayTime(totalPlayTimeCounter.TotalPlayTime);
            uIManager.ShowTetrisDropped(mainCotrolUnit.TotalTetrisDropped);
        }
    }
}