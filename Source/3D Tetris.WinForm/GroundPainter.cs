using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace _3D_Tetris.WinForm
{
    internal class GroundPainter : IDisposable
    {
        private readonly SolidBrush edgeBrush;
        //private readonly Pen midPen;
        private readonly SolidBrush groundBrush;
        private Image? ground;
        private PointF groundOrigin;

        private Point unitXProjection;
        private Point unitYProjection;
        private SizeF groundSize;
        private int xAxis;
        private int yAxis;

        private readonly PointF[] insidePoints = new PointF[4];
        private readonly PointF[] outsidePoints = new PointF[4];

        public GroundPainter()
        {
            edgeBrush = new SolidBrush(Color.FromArgb(GameConfigData.GroundEdgeColorR, GameConfigData.GroundEdgeColorG, GameConfigData.GroundEdgeColorB));
            //midPen = new Pen(Color.FromArgb(0x92, 0x9A, 0xAB), 0.5f);
            groundBrush = new SolidBrush(Color.FromArgb(0x39, 0x3E, 0x46));
        }

        public SizeF GroundSize { get => groundSize; }

        public bool ConfigureProjection(Point unitXProjection, Point unitYProjection, int xAxis, int yAxis)
        {
            this.unitXProjection = unitXProjection;
            this.unitYProjection = unitYProjection;
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            return GenerateUnitGround();
        }

        public void Dispose()
        {
            edgeBrush.Dispose();
            groundBrush.Dispose();
            ground?.Dispose();
        }

        public void PaintGround(Image? canvas, Point anchor, bool doYAxisReflection)
        {
            if (canvas == null || ground == null)
            {
                return;
            }

            Graphics g = Graphics.FromImage(canvas);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            PointF reflectedAnchor = (PointF)anchor + new SizeF(groundOrigin.X, -groundOrigin.Y) - new SizeF(unitXProjection.X * yAxis * 2, 0);
            if (doYAxisReflection)
            {
                //g.DrawImage(ground, modifiedAnchor.X + yAxis * unitYProjection.Y * 2, modifiedAnchor.Y, -ground.Width, ground.Height);
                g.DrawImage(ground, reflectedAnchor.X, reflectedAnchor.Y, -ground.Width, ground.Height);
                return;
            }

            g.DrawImage(ground, (PointF)anchor - new SizeF(groundOrigin));
            return;
        }

        private bool GenerateUnitGround()
        {
            ground?.Dispose();

            if (xAxis < 1 || yAxis < 1 || unitXProjection == default || unitYProjection == default)
            {
                return false;
            }

            float groundEdgeHeight = GameConfigData.GroundEdgeWidth;

            insidePoints[0] = new PointF();
            insidePoints[1] = (Point)(new Size(unitXProjection) * xAxis);
            insidePoints[2] = (Point)(new Size(unitXProjection) * xAxis + new Size(unitYProjection) * yAxis);
            insidePoints[3] = (Point)(new Size(unitYProjection) * yAxis);
            outsidePoints[0] = CaculateOutsidePoint(insidePoints[0], insidePoints[1] - new SizeF(insidePoints[0]), insidePoints[3] - new SizeF(insidePoints[0]), groundEdgeHeight);
            outsidePoints[1] = CaculateOutsidePoint(insidePoints[1], insidePoints[2] - new SizeF(insidePoints[1]), insidePoints[0] - new SizeF(insidePoints[1]), groundEdgeHeight);
            outsidePoints[2] = CaculateOutsidePoint(insidePoints[2], insidePoints[3] - new SizeF(insidePoints[2]), insidePoints[1] - new SizeF(insidePoints[2]), groundEdgeHeight);
            outsidePoints[3] = CaculateOutsidePoint(insidePoints[3], insidePoints[0] - new SizeF(insidePoints[3]), insidePoints[2] - new SizeF(insidePoints[3]), groundEdgeHeight);
            
            PointF outsideMinPoint = new(MathF.Min(MathF.Min(outsidePoints[0].X, outsidePoints[1].X), MathF.Min(outsidePoints[2].X, outsidePoints[3].X)), Math.Min(MathF.Min(outsidePoints[0].Y, outsidePoints[1].Y), MathF.Min(outsidePoints[2].Y, outsidePoints[3].Y)));
            PointF outsideMaxPoint = new(MathF.Max(MathF.Max(outsidePoints[0].X, outsidePoints[1].X), MathF.Max(outsidePoints[2].X, outsidePoints[3].X)), Math.Max(MathF.Max(outsidePoints[0].Y, outsidePoints[1].Y), MathF.Max(outsidePoints[2].Y, outsidePoints[3].Y)));
            
            //PointF insideMinPoint = new(MathF.Min(MathF.Min(insidePoints[0].X, insidePoints[1].X), MathF.Min(insidePoints[2].X, insidePoints[3].X)), Math.Min(MathF.Min(insidePoints[0].Y, insidePoints[1].Y), MathF.Min(insidePoints[2].Y, insidePoints[3].Y)));
            //PointF insideMaxPoint = new(MathF.Max(MathF.Max(insidePoints[0].X, insidePoints[1].X), MathF.Max(insidePoints[2].X, insidePoints[3].X)), Math.Max(MathF.Max(insidePoints[0].Y, insidePoints[1].Y), MathF.Max(insidePoints[2].Y, insidePoints[3].Y)));
            //insideWidth = insideMaxPoint.X - insideMinPoint.X;

            groundOrigin = new PointF(-MathF.Min(outsideMinPoint.X, 0), -MathF.Min(outsideMinPoint.Y, 0));
            groundSize = new SizeF(outsideMaxPoint.X - outsideMinPoint.X, outsideMaxPoint.Y - outsideMinPoint.Y);
            ground = new Bitmap((int)groundSize.Width, (int)groundSize.Height);

            for (int i = 0; i < 4; i++)
            {
                insidePoints[i] += new SizeF(groundOrigin);
                outsidePoints[i] += new SizeF(groundOrigin);
            }

            Graphics g = Graphics.FromImage(ground);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Transparent);
            g.FillPolygon(edgeBrush, outsidePoints);
            g.FillPolygon(groundBrush, insidePoints);

            //for (int i = 1; i < xAxis; i++)
            //{
            //    Size xDisplacement = new Size(unitXProjection.X * i, unitXProjection.Y * i);
            //    g.DrawLine(midPen, groundOrigin + xDisplacement, groundOrigin + xDisplacement + new Size(unitYProjection) * yAxis);
            //}
            //for (int i = 1; i < yAxis; i++)
            //{
            //    Size yDisplacement = new Size(unitYProjection.X * i, unitYProjection.Y * i);
            //    g.DrawLine(midPen, groundOrigin + yDisplacement, groundOrigin + yDisplacement + new Size(unitXProjection) * xAxis);
            //}
            return true;
        }

        private static PointF CaculateOutsidePoint(PointF insidePoint, PointF vector1, PointF vector2, float width)
        {
            PointF unitVector1 = Normalize(vector1);
            PointF ortoVector1 = new (unitVector1.Y, -unitVector1.X);
            if (InnerProduct(ortoVector1, vector2) > 0)
            {
                ortoVector1 = new(-unitVector1.Y, unitVector1.X);
            }

            float cosTheta = InnerProduct(unitVector1, Normalize(vector2));
            float cotHalfTheta = MathF.Sqrt((1 + cosTheta) / (1 - cosTheta));
            return new PointF(insidePoint.X, insidePoint.Y) + new SizeF(ortoVector1) * width - new SizeF(unitVector1) * width * cotHalfTheta;
        }

        private static float InnerProduct(PointF vector1, PointF vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        private static PointF Normalize(PointF vector1)
        {
            return new(vector1.X / MathF.Sqrt(vector1.Y * vector1.Y + vector1.X * vector1.X), vector1.Y / MathF.Sqrt(vector1.Y * vector1.Y + vector1.X * vector1.X));
        }
    }
}
