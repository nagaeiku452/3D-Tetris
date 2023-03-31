
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MainGame.Physics.StaticGridSystem;
using MainGame.Physics.Blocking;
using MainGame.Numeric;
using Timer = System.Windows.Forms.Timer;

namespace _3D_Tetris.WinForm
{
    partial class MainWindow : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private readonly Timer gameLoopTimer = new Timer() { Interval = 1 };
        private readonly Timer renderLoopTimer = new Timer() { Interval = 1 };


        public int GameLoopInterval
        {
            get => gameLoopTimer.Interval;
            set => gameLoopTimer.Interval = value;
        }
        public int RenderLoopInterval
        {
            get => renderLoopTimer.Interval;
            set => renderLoopTimer.Interval = value;
        }

        //public EventHandler GameLoopEvent;
        //public EventHandler RenderLoopEvent;

        ThreeDimTetrisControl curControlUnit;

        //internal StaticGridDynamicWorld<TetrisBodyBase> World { get; private set; }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnActivated(e);
            //this.SetStyle(ControlStyles.FixedHeight, true);
            //this.SetStyle(ControlStyles.FixedWidth, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.Opaque, false);
            //this.UpdateStyles();

            //init timer
            //gameLoopTimer.Tick += OnGameLoopTick;
            //renderLoopTimer.Tick += OnRenderLoopTick;

            //init world

            //init control unit
            curControlUnit = new ThreeDimTetrisControl(this);
            gameLoopTimer.Tick += OnGameLoopEvent;
            renderLoopTimer.Tick += OnRenderLoopEvent;
            KeyDown += curControlUnit.OnKeyDown;
            KeyUp += curControlUnit.OnKeyUp;


            //start the timer
            gameLoopTimer.Start();
            renderLoopTimer.Start();


            //testTetris = new TetrisBodyBase(StaticGridRigidBodyType.Dynamic);
            //testTetris.ForceSetActivationState(GridBodyActivationState.DisableDeactivation);
            //testTetris.AffectByGravity = false;
            //testTetris.WorldTransform = new Vector3i(1, 1, 10);
            //testTetris.GridCollisionShape = item.Item1;
            //testTetris.PaintColor = item.Item2;
            //world.AddRigidBody(testTetris);
            //foreach ((GridCollisionShape, Color) item in TetraCubeShapePrototypeProvider.TetraCubeShapes)
            //{
            //    TetrisBodyBase t = new TetrisBodyBase(StaticGridRigidBodyType.Dynamic);
            //    t.ForceSetActivationState(GridBodyActivationState.DisableDeactivation);
            //    t.AffectByGravity = false;
            //    t.WorldTransform = MainGame.Numeric.Vector3i.UnitZ * (2 * i - 1) + MainGame.Numeric.Vector3i.One;
            //    t.GridCollisionShape = item.Item1;
            //    t.PaintColor = item.Item2;
            //    //if(item.Item2 == Color.Orange)
            //    {
            //        world.AddRigidBody(t);
            //        i++;
            //    }
            //}

            GameWindow.BackColor = Color.Transparent;
            //Image i = BWBoxSpriteGenerator.GenerateBWBoxSprite(100, 150, 100, 1, 100, 200);



            //BWBoxSprite spriteUnitAsset = new BWBoxSprite();
            //spriteUnitAsset.GenerateImage(50, 50, 100, 40, 120, 200);

            //GroundPainter groundPainter = new GroundPainter();
            //groundPainter.ConfigureProjection(new Point(spriteUnitAsset.ZSurfaceWidth / 2, spriteUnitAsset.ZSurfaceHeight / 2), new Point(spriteUnitAsset.ZSurfaceWidth / 2, -spriteUnitAsset.ZSurfaceHeight / 2), 4, 4);
            //groundPainter.PaintGround(GameWindow.Image, new Point(GameWindow.Size.Width / 2, GameWindow.Size.Height / 2));

            //WorldViewBox viewBox = new WorldViewBox(new MainGame.Numeric.Vector3i(), new MainGame.Numeric.Vector3i(4, 4, 4));

            ////GameWindow.Image = spriteUnitAsset.Sprite;
            ////ImageAttributes imageAttributes = new ImageAttributes();
            ////ColorMatrix matrix = new ColorMatrix(); 
            ////ColorMatrixGenerator.GenerateColorMatrix(Color.Red, matrix);
            ////imageAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            //WorldObjectPainter painter = new WorldObjectPainter(spriteUnitAsset, viewBox);
            //painter.PaintWorld(world, GameWindow.Image, new Point(GameWindow.Size.Width / 2 - 200, GameWindow.Size.Height / 2 - 200), CameraViewAngle.firstQuadrant);
            //painter.PaintWorld(world, GameWindow.Image, new Point(GameWindow.Size.Width / 2 - 200, GameWindow.Size.Height / 2), CameraViewAngle.secondQuadrant);
            //painter.PaintWorld(world, GameWindow.Image, new Point(GameWindow.Size.Width / 2, GameWindow.Size.Height / 2 - 200), CameraViewAngle.thirdQuadrant);
            //painter.PaintWorld(world, GameWindow.Image, new Point(GameWindow.Size.Width / 2, GameWindow.Size.Height / 2), CameraViewAngle.firstQuadrant);
            //g.DrawImage(spriteUnitAsset.Sprite, new Rectangle(default, spriteUnitAsset.Sprite.Size), 15, 15, spriteUnitAsset.Sprite.Width, spriteUnitAsset.Sprite.Height, GraphicsUnit.Pixel, imageAttributes);
            //g.DrawImage(i, new Rectangle(default, i.Size), 150, 150, i.Width, i.Height, GraphicsUnit.Pixel, imageAttributes);

        }

        private void OnRenderLoopEvent(object sender, EventArgs e)
        {
            curControlUnit.OnRenderLoopEvent();
        }

        private void OnGameLoopEvent(object sender, EventArgs e)
        {
            curControlUnit.OnGameLoopEvent();
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    curControlUnit?.OnKeyInput(this, e);
        //}

        //private void OnRenderLoopTick(object sender, EventArgs e)
        //{
        //    RenderLoopEvent.Invoke(sender, e);
        //}

        //private void OnGameLoopTick(object sender, EventArgs e)
        //{
        //    GameLoopEvent.Invoke(sender, e);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GameWindow = new PictureBox();
            Score = new Label();
            NextTetris1 = new Label();
            NextTetris2 = new Label();
            Log = new TextBox();
            NextTetris3 = new Label();
            WorldOrigin = new Label();
            Level = new Label();
            SliceCleared = new Label();
            PlayedTime = new Label();
            HoldTetris = new Label();
            StatisicsPanel = new Panel();
            TotalTetrisDropped = new Label();
            Hold = new Label();
            Next = new Label();
            ((System.ComponentModel.ISupportInitialize)GameWindow).BeginInit();
            StatisicsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // GameWindow
            // 
            GameWindow.Enabled = false;
            GameWindow.Location = new Point(0, 0);
            GameWindow.Margin = new Padding(0);
            GameWindow.Name = "GameWindow";
            GameWindow.Size = new Size(1280, 720);
            GameWindow.TabIndex = 1;
            GameWindow.TabStop = false;
            // 
            // Score
            // 
            Score.AutoEllipsis = true;
            Score.AutoSize = true;
            Score.BackColor = Color.Transparent;
            Score.Font = new Font("Miriam Mono CLM", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Score.ForeColor = Color.White;
            Score.Location = new Point(0, 28);
            Score.Name = "Score";
            Score.Size = new Size(217, 28);
            Score.TabIndex = 2;
            Score.Text = "current score:100";
            Score.UseCompatibleTextRendering = true;
            // 
            // NextTetris1
            // 
            NextTetris1.AutoSize = true;
            NextTetris1.Location = new Point(370, 230);
            NextTetris1.Name = "NextTetris1";
            NextTetris1.Size = new Size(71, 15);
            NextTetris1.TabIndex = 3;
            NextTetris1.Text = "NextTetris1";
            NextTetris1.Visible = false;
            // 
            // NextTetris2
            // 
            NextTetris2.AutoSize = true;
            NextTetris2.Location = new Point(370, 330);
            NextTetris2.Name = "NextTetris2";
            NextTetris2.Size = new Size(71, 15);
            NextTetris2.TabIndex = 4;
            NextTetris2.Text = "NextTetris2";
            NextTetris2.Visible = false;
            // 
            // Log
            // 
            Log.Enabled = false;
            Log.Location = new Point(978, 399);
            Log.Multiline = true;
            Log.Name = "Log";
            Log.Size = new Size(100, 188);
            Log.TabIndex = 5;
            Log.Text = "Log";
            // 
            // NextTetris3
            // 
            NextTetris3.AutoSize = true;
            NextTetris3.Location = new Point(370, 430);
            NextTetris3.Name = "NextTetris3";
            NextTetris3.Size = new Size(71, 15);
            NextTetris3.TabIndex = 6;
            NextTetris3.Text = "NextTetris3";
            NextTetris3.Visible = false;
            // 
            // WorldOrigin
            // 
            WorldOrigin.AutoSize = true;
            WorldOrigin.Enabled = false;
            WorldOrigin.Location = new Point(635, 515);
            WorldOrigin.Name = "WorldOrigin";
            WorldOrigin.Size = new Size(77, 15);
            WorldOrigin.TabIndex = 7;
            WorldOrigin.Text = "WorldOrigin";
            WorldOrigin.Visible = false;
            // 
            // Level
            // 
            Level.AutoEllipsis = true;
            Level.AutoSize = true;
            Level.BackColor = Color.Transparent;
            Level.Font = new Font("Miriam Mono CLM", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Level.ForeColor = Color.White;
            Level.Location = new Point(0, 0);
            Level.Name = "Level";
            Level.Size = new Size(94, 28);
            Level.TabIndex = 8;
            Level.Text = "level:1";
            Level.UseCompatibleTextRendering = true;
            // 
            // SliceCleared
            // 
            SliceCleared.AutoSize = true;
            SliceCleared.BackColor = Color.Transparent;
            SliceCleared.Font = new Font("Miriam Mono CLM", 15F, FontStyle.Regular, GraphicsUnit.Point);
            SliceCleared.ForeColor = Color.White;
            SliceCleared.Location = new Point(0, 56);
            SliceCleared.Name = "SliceCleared";
            SliceCleared.Size = new Size(230, 28);
            SliceCleared.TabIndex = 9;
            SliceCleared.Text = "sliced cleared:100";
            SliceCleared.UseCompatibleTextRendering = true;
            // 
            // PlayedTime
            // 
            PlayedTime.AutoSize = true;
            PlayedTime.BackColor = Color.Transparent;
            PlayedTime.Font = new Font("Miriam Mono CLM", 15F, FontStyle.Regular, GraphicsUnit.Point);
            PlayedTime.ForeColor = Color.White;
            PlayedTime.Location = new Point(0, 84);
            PlayedTime.Name = "PlayedTime";
            PlayedTime.Size = new Size(267, 28);
            PlayedTime.TabIndex = 10;
            PlayedTime.Text = "Played time:100:00:00";
            PlayedTime.UseCompatibleTextRendering = true;
            // 
            // HoldTetris
            // 
            HoldTetris.AutoSize = true;
            HoldTetris.Location = new Point(370, 70);
            HoldTetris.Name = "HoldTetris";
            HoldTetris.Size = new Size(65, 15);
            HoldTetris.TabIndex = 11;
            HoldTetris.Text = "HoldTetris";
            HoldTetris.Visible = false;
            // 
            // StatisicsPanel
            // 
            StatisicsPanel.BackColor = Color.Transparent;
            StatisicsPanel.Controls.Add(TotalTetrisDropped);
            StatisicsPanel.Controls.Add(Level);
            StatisicsPanel.Controls.Add(Score);
            StatisicsPanel.Controls.Add(PlayedTime);
            StatisicsPanel.Controls.Add(SliceCleared);
            StatisicsPanel.ForeColor = Color.Transparent;
            StatisicsPanel.Location = new Point(833, 106);
            StatisicsPanel.Name = "StatisicsPanel";
            StatisicsPanel.Size = new Size(319, 259);
            StatisicsPanel.TabIndex = 12;
            // 
            // TotalTetrisDropped
            // 
            TotalTetrisDropped.AutoSize = true;
            TotalTetrisDropped.BackColor = Color.Transparent;
            TotalTetrisDropped.Font = new Font("Miriam Mono CLM", 15F, FontStyle.Regular, GraphicsUnit.Point);
            TotalTetrisDropped.ForeColor = Color.White;
            TotalTetrisDropped.Location = new Point(0, 112);
            TotalTetrisDropped.Name = "TotalTetrisDropped";
            TotalTetrisDropped.Size = new Size(230, 28);
            TotalTetrisDropped.TabIndex = 11;
            TotalTetrisDropped.Text = "Tetris Dropped:100";
            TotalTetrisDropped.UseCompatibleTextRendering = true;
            // 
            // Hold
            // 
            Hold.AutoSize = true;
            Hold.Font = new Font("Segoe UI Symbol", 15F, FontStyle.Bold, GraphicsUnit.Point);
            Hold.Location = new Point(250, 70);
            Hold.Name = "Hold";
            Hold.Size = new Size(59, 28);
            Hold.TabIndex = 13;
            Hold.Text = "Hold";
            // 
            // Next
            // 
            Next.AutoSize = true;
            Next.Font = new Font("Segoe UI Symbol", 15F, FontStyle.Bold, GraphicsUnit.Point);
            Next.Location = new Point(250, 230);
            Next.Name = "Next";
            Next.Size = new Size(57, 28);
            Next.TabIndex = 14;
            Next.Text = "Next";
            // 
            // MainWindow
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(1264, 681);
            ControlBox = false;
            Controls.Add(Next);
            Controls.Add(Hold);
            Controls.Add(NextTetris3);
            Controls.Add(NextTetris2);
            Controls.Add(NextTetris1);
            Controls.Add(StatisicsPanel);
            Controls.Add(HoldTetris);
            Controls.Add(WorldOrigin);
            Controls.Add(Log);
            Controls.Add(GameWindow);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainWindow";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "3D Tetris";
            ((System.ComponentModel.ISupportInitialize)GameWindow).EndInit();
            StatisicsPanel.ResumeLayout(false);
            StatisicsPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public System.Windows.Forms.PictureBox GameWindow;
        public Label Score;
        public Label NextTetris1;
        public Label NextTetris2;
        public TextBox Log;
        public Label NextTetris3;
        public Label WorldOrigin;
        public Label Level;
        public Label SliceCleared;
        public Label PlayedTime;
        public Label HoldTetris;
        public Panel StatisicsPanel;
        public Label Hold;
        public Label Next;
        public Label TotalTetrisDropped;
    }
}