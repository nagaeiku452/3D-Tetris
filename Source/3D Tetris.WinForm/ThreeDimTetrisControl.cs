using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System.Reflection.Emit;

namespace _3D_Tetris.WinForm
{
    internal class ThreeDimTetrisControl : WinFormControlUnit
    {
        private Image frontImage;
        private Image backImage;

        private readonly WorldViewPainter worldViewPainter;

        private readonly SingleTetrisPainter nextSingleTetrisPainter;
        private readonly SingleTetrisPainter heldSingleTetrisPainter;
        private readonly Point[] nextTetrisPos = new Point[3];

        private readonly ThreeDimTetrisControlUnit mainCotrolUnit;
        private readonly ThreeDimTetrisUIManager uIManager;

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

            nextTetrisPos[0] = mainWindow.NextTetris1.Location;
            nextTetrisPos[1] = mainWindow.NextTetris2.Location;
            nextTetrisPos[2] = mainWindow.NextTetris3.Location;

            uIManager = new(mainWindow);
            mainCotrolUnit = new ThreeDimTetrisControlUnit(viewBoxMax);
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

        public override void OnGameLoopEvent(int millisecondPassed)
        {
            mainCotrolUnit.OnGameLoopEvent(millisecondPassed);
        }

        public override void OnRenderLoopEvent(int millisecondPassed)
        {
            base.OnRenderLoopEvent(millisecondPassed);


            //clear background
            Graphics g = Graphics.FromImage(backImage);
            g.Clear(GameConfigData.BackgroundColor);

            //paint world
            worldViewPainter.PaintWorld(mainCotrolUnit.TetrisWorld, backImage, mainWindow.WorldOrigin.Location, mainCotrolUnit.CurViewAngle);

            //paint next tetrises
            int i = 0;
            foreach ((GridCollisionShape, Color) item in mainCotrolUnit.NextTetrisData.Select(v => ((GridCollisionShape, Color))v))
            {
                if (i < 3)
                {
                    PaintSingleTetris(nextSingleTetrisPainter, backImage, nextTetrisPos[i], item.Item1, item.Item2, false);
                }
                i++;
            }

            //paint hold tetris

            PaintSingleTetris(heldSingleTetrisPainter, backImage, mainWindow.HoldTetris.Location, mainCotrolUnit.HeldTetris.GridCollisionShape, mainCotrolUnit.HeldTetris.PaintColor, mainCotrolUnit.IsHeldTetris);

            //switch
            (backImage, frontImage) = (frontImage, backImage);
            mainWindow.GameWindow.BackgroundImage = frontImage;

            ShowUIData();

            mainWindow.Log.Text = $"{mainCotrolUnit.TotalTetrisDropped / mainCotrolUnit.TotalPlayTime.TotalSeconds:F2}";
        }

        private static void PaintSingleTetris(SingleTetrisPainter bigSingleTetrisPainter, Image backImage, Point anchor, GridCollisionShape shape, Color color, bool IsHeld)
        {
            bigSingleTetrisPainter.PaintSingleTetris(backImage, anchor, shape, color, IsHeld);
        }


        public override void Dispose()
        {

        }

        private void ShowUIData()
        {
            uIManager.ShowScore(mainCotrolUnit.CurrentScore);
            uIManager.ShowSliceCleared(mainCotrolUnit.CurrentClearedSlices);
            uIManager.ShowLevel(mainCotrolUnit.CurrentLevel);
            uIManager.ShowPlayTime(mainCotrolUnit.TotalPlayTime);
            uIManager.ShowTetrisDropped(mainCotrolUnit.TotalTetrisDropped);
        }
    }
}