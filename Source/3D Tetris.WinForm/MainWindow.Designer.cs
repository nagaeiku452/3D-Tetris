
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

        private readonly Timer gameLoopTimer = new Timer() { Interval = GameConfigData.GameLoopInterval };
        private readonly Timer renderLoopTimer = new Timer() { Interval = GameConfigData.RenderInterval };


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
            curControlUnit.OnRenderLoopEvent(RenderLoopInterval);
        }

        private void OnGameLoopEvent(object sender, EventArgs e)
        {
            curControlUnit.OnGameLoopEvent(GameLoopInterval);
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
            this.GameWindow = new System.Windows.Forms.PictureBox();
            this.Score = new System.Windows.Forms.Label();
            this.NextTetris1 = new System.Windows.Forms.Label();
            this.NextTetris2 = new System.Windows.Forms.Label();
            this.Log = new System.Windows.Forms.TextBox();
            this.NextTetris3 = new System.Windows.Forms.Label();
            this.WorldOrigin = new System.Windows.Forms.Label();
            this.Level = new System.Windows.Forms.Label();
            this.SliceCleared = new System.Windows.Forms.Label();
            this.PlayedTime = new System.Windows.Forms.Label();
            this.HoldTetris = new System.Windows.Forms.Label();
            this.StatisicsPanel = new System.Windows.Forms.Panel();
            this.Hold = new System.Windows.Forms.Label();
            this.Next = new System.Windows.Forms.Label();
            this.TotalTetrisDropped = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GameWindow)).BeginInit();
            this.StatisicsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GameWindow
            // 
            this.GameWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.GameWindow.Enabled = false;
            this.GameWindow.Location = new System.Drawing.Point(0, 0);
            this.GameWindow.Margin = new System.Windows.Forms.Padding(0);
            this.GameWindow.Name = "GameWindow";
            this.GameWindow.Size = new System.Drawing.Size(1280, 720);
            this.GameWindow.TabIndex = 1;
            this.GameWindow.TabStop = false;
            // 
            // Score
            // 
            this.Score.AutoEllipsis = true;
            this.Score.AutoSize = true;
            this.Score.BackColor = System.Drawing.Color.Transparent;
            this.Score.Font = new System.Drawing.Font("Miriam Mono CLM", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Score.ForeColor = System.Drawing.Color.White;
            this.Score.Location = new System.Drawing.Point(0, 28);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(217, 28);
            this.Score.TabIndex = 2;
            this.Score.Text = "current score:100";
            this.Score.UseCompatibleTextRendering = true;
            // 
            // NextTetris1
            // 
            this.NextTetris1.AutoSize = true;
            this.NextTetris1.Location = new System.Drawing.Point(370, 230);
            this.NextTetris1.Name = "NextTetris1";
            this.NextTetris1.Size = new System.Drawing.Size(71, 15);
            this.NextTetris1.TabIndex = 3;
            this.NextTetris1.Text = "NextTetris1";
            this.NextTetris1.Visible = false;
            // 
            // NextTetris2
            // 
            this.NextTetris2.AutoSize = true;
            this.NextTetris2.Location = new System.Drawing.Point(370, 330);
            this.NextTetris2.Name = "NextTetris2";
            this.NextTetris2.Size = new System.Drawing.Size(71, 15);
            this.NextTetris2.TabIndex = 4;
            this.NextTetris2.Text = "NextTetris2";
            this.NextTetris2.Visible = false;
            // 
            // Log
            // 
            this.Log.Enabled = false;
            this.Log.Location = new System.Drawing.Point(978, 399);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(100, 188);
            this.Log.TabIndex = 5;
            this.Log.Text = "Log";
            // 
            // NextTetris3
            // 
            this.NextTetris3.AutoSize = true;
            this.NextTetris3.Location = new System.Drawing.Point(370, 430);
            this.NextTetris3.Name = "NextTetris3";
            this.NextTetris3.Size = new System.Drawing.Size(71, 15);
            this.NextTetris3.TabIndex = 6;
            this.NextTetris3.Text = "NextTetris3";
            this.NextTetris3.Visible = false;
            // 
            // WorldOrigin
            // 
            this.WorldOrigin.AutoSize = true;
            this.WorldOrigin.Enabled = false;
            this.WorldOrigin.Location = new System.Drawing.Point(635, 515);
            this.WorldOrigin.Name = "WorldOrigin";
            this.WorldOrigin.Size = new System.Drawing.Size(77, 15);
            this.WorldOrigin.TabIndex = 7;
            this.WorldOrigin.Text = "WorldOrigin";
            this.WorldOrigin.Visible = false;
            // 
            // Level
            // 
            this.Level.AutoEllipsis = true;
            this.Level.AutoSize = true;
            this.Level.BackColor = System.Drawing.Color.Transparent;
            this.Level.Font = new System.Drawing.Font("Miriam Mono CLM", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Level.ForeColor = System.Drawing.Color.White;
            this.Level.Location = new System.Drawing.Point(0, 0);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(94, 28);
            this.Level.TabIndex = 8;
            this.Level.Text = "level:1";
            this.Level.UseCompatibleTextRendering = true;
            // 
            // SliceCleared
            // 
            this.SliceCleared.AutoSize = true;
            this.SliceCleared.BackColor = System.Drawing.Color.Transparent;
            this.SliceCleared.Font = new System.Drawing.Font("Miriam Mono CLM", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SliceCleared.ForeColor = System.Drawing.Color.White;
            this.SliceCleared.Location = new System.Drawing.Point(0, 56);
            this.SliceCleared.Name = "SliceCleared";
            this.SliceCleared.Size = new System.Drawing.Size(230, 28);
            this.SliceCleared.TabIndex = 9;
            this.SliceCleared.Text = "sliced cleared:100";
            this.SliceCleared.UseCompatibleTextRendering = true;
            // 
            // PlayedTime
            // 
            this.PlayedTime.AutoSize = true;
            this.PlayedTime.BackColor = System.Drawing.Color.Transparent;
            this.PlayedTime.Font = new System.Drawing.Font("Miriam Mono CLM", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayedTime.ForeColor = System.Drawing.Color.White;
            this.PlayedTime.Location = new System.Drawing.Point(0, 84);
            this.PlayedTime.Name = "PlayedTime";
            this.PlayedTime.Size = new System.Drawing.Size(267, 28);
            this.PlayedTime.TabIndex = 10;
            this.PlayedTime.Text = "Played time:100:00:00";
            this.PlayedTime.UseCompatibleTextRendering = true;
            // 
            // HoldTetris
            // 
            this.HoldTetris.AutoSize = true;
            this.HoldTetris.Location = new System.Drawing.Point(370, 70);
            this.HoldTetris.Name = "HoldTetris";
            this.HoldTetris.Size = new System.Drawing.Size(65, 15);
            this.HoldTetris.TabIndex = 11;
            this.HoldTetris.Text = "HoldTetris";
            this.HoldTetris.Visible = false;
            // 
            // StatisicsPanel
            // 
            this.StatisicsPanel.BackColor = System.Drawing.Color.Transparent;
            this.StatisicsPanel.Controls.Add(this.TotalTetrisDropped);
            this.StatisicsPanel.Controls.Add(this.Level);
            this.StatisicsPanel.Controls.Add(this.Score);
            this.StatisicsPanel.Controls.Add(this.PlayedTime);
            this.StatisicsPanel.Controls.Add(this.SliceCleared);
            this.StatisicsPanel.ForeColor = System.Drawing.Color.Transparent;
            this.StatisicsPanel.Location = new System.Drawing.Point(833, 106);
            this.StatisicsPanel.Name = "StatisicsPanel";
            this.StatisicsPanel.Size = new System.Drawing.Size(319, 259);
            this.StatisicsPanel.TabIndex = 12;
            // 
            // Hold
            // 
            this.Hold.AutoSize = true;
            this.Hold.Font = new System.Drawing.Font("Segoe UI Symbol", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Hold.Location = new System.Drawing.Point(250, 70);
            this.Hold.Name = "Hold";
            this.Hold.Size = new System.Drawing.Size(59, 28);
            this.Hold.TabIndex = 13;
            this.Hold.Text = "Hold";
            // 
            // Next
            // 
            this.Next.AutoSize = true;
            this.Next.Font = new System.Drawing.Font("Segoe UI Symbol", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Next.Location = new System.Drawing.Point(250, 230);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(57, 28);
            this.Next.TabIndex = 14;
            this.Next.Text = "Next";
            // 
            // TotalTetrisDropped
            // 
            this.TotalTetrisDropped.AutoSize = true;
            this.TotalTetrisDropped.BackColor = System.Drawing.Color.Transparent;
            this.TotalTetrisDropped.Font = new System.Drawing.Font("Miriam Mono CLM", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TotalTetrisDropped.ForeColor = System.Drawing.Color.White;
            this.TotalTetrisDropped.Location = new System.Drawing.Point(0, 112);
            this.TotalTetrisDropped.Name = "TotalTetrisDropped";
            this.TotalTetrisDropped.Size = new System.Drawing.Size(230, 28);
            this.TotalTetrisDropped.TabIndex = 11;
            this.TotalTetrisDropped.Text = "Tetris Dropped:100";
            this.TotalTetrisDropped.UseCompatibleTextRendering = true;
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.ControlBox = false;
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Hold);
            this.Controls.Add(this.NextTetris3);
            this.Controls.Add(this.NextTetris2);
            this.Controls.Add(this.NextTetris1);
            this.Controls.Add(this.StatisicsPanel);
            this.Controls.Add(this.HoldTetris);
            this.Controls.Add(this.WorldOrigin);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.GameWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "3D Tetris";
            ((System.ComponentModel.ISupportInitialize)(this.GameWindow)).EndInit();
            this.StatisicsPanel.ResumeLayout(false);
            this.StatisicsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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