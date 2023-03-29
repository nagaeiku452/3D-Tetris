using MainGame.Numeric;
using System;
using System.Collections.Generic;

namespace _3D_Tetris.WinForm
{
    internal class CameraViewPlane
    {
        private readonly PaintDataComparison comparison;
        private readonly List<(Vector3i, TetrisBodyBase)> paintDataBuffer;
        private readonly IDictionary<int, (Point, TetrisBodyBase)> paintData = new Dictionary<int, (Point, TetrisBodyBase)>();
        private readonly IDictionary<Point, int> orderData = new Dictionary<Point, int>();
        private readonly ISet<int> orderSet = new HashSet<int>();

        public CameraViewAngle CurrentViewAngle
        {
            get => comparison.CurrentViewAngle;
            set => comparison.CurrentViewAngle = value;
        }

        internal CameraViewPlane()
        {
            comparison = new PaintDataComparison(CameraViewAngle.firstQuadrant);
            paintDataBuffer = new List<(Vector3i, TetrisBodyBase)>();
        }

        public IEnumerable<(Point, TetrisBodyBase)> GenerateCameraView(IEnumerable<KeyValuePair<Vector3i, TetrisBodyBase>> rawPaintData, Vector3i min, Vector3i max, CameraViewAngle viewAngle, Point unitXProjection, Point unitYProjection, Point unitZProjection)
        {
            comparison.CurrentViewAngle = viewAngle;

            Matrix3x3i rotationMatrix = viewAngle.GenerateRotationMatrix();

            paintDataBuffer.Clear();
            paintData.Clear();
            orderData.Clear();

            foreach (KeyValuePair<Vector3i, TetrisBodyBase> item in rawPaintData)
            {
                paintDataBuffer.Add((item.Key, item.Value));
            }
            paintDataBuffer.Sort(comparison);
            Vector3i rotatedMin = rotationMatrix * min;
            Vector3i rotatedMax = rotationMatrix * max;

            Vector3i TransLation = new(Math.Min(rotatedMin.X, rotatedMax.X), Math.Min(rotatedMin.Y, rotatedMax.Y), Math.Min(rotatedMin.Z, rotatedMax.Z));

            //newMin is Vector.Zero
            //Vector3i newMax = new Vector3i(Math.Max(rotatedMin.X, rotatedMax.X), Math.Max(rotatedMin.Y, rotatedMax.Y), Math.Max(rotatedMin.Z, rotatedMax.Z)) - TransLation;

            for (int i = 0; i < paintDataBuffer.Count; i++)
            {
                (Vector3i, TetrisBodyBase) sortedPaintData = paintDataBuffer[i];
                Vector3i v = rotationMatrix * sortedPaintData.Item1 - TransLation;
                Point p = new(v.X * unitXProjection.X + v.Y * unitYProjection.X + v.Z * unitZProjection.X, v.X * unitXProjection.Y + v.Y * unitYProjection.Y + v.Z * unitZProjection.Y);

                paintData[i] = (p, sortedPaintData.Item2);
                orderData[p] = i;
            }

            orderSet.Clear();
            foreach (KeyValuePair<Point, int> item in orderData)
            {
                orderSet.Add(item.Value);
            }

            for (int i = 0; i < paintDataBuffer.Count; i++)
            {
                if (orderSet.Contains(i))
                {
                    yield return paintData[i];
                }
            }

        }


        private class PaintDataComparison : IComparer<(Vector3i, TetrisBodyBase)>
        {
            public CameraViewAngle CurrentViewAngle { get; set; }

            public PaintDataComparison(CameraViewAngle initViewAngle)
            {
                CurrentViewAngle = initViewAngle;
            }

            public int Compare((Vector3i, TetrisBodyBase) x, (Vector3i, TetrisBodyBase) y)
            {
                return CurrentViewAngle switch
                {
                    CameraViewAngle.firstQuadrant => (x.Item1.X + x.Item1.Y + x.Item1.Z - (y.Item1.X + y.Item1.Y + y.Item1.Z)) * 3 - (x.Item2.IsShadow ? 1 : 0) + (y.Item2.IsShadow ? 1 : 0),
                    CameraViewAngle.secondQuadrant => (-x.Item1.X + x.Item1.Y + x.Item1.Z - (-y.Item1.X + y.Item1.Y + y.Item1.Z)) * 3 - (x.Item2.IsShadow ? 1 : 0) + (y.Item2.IsShadow ? 1 : 0),
                    CameraViewAngle.thirdQuadrant => (-x.Item1.X - x.Item1.Y + x.Item1.Z - (-y.Item1.X - y.Item1.Y + y.Item1.Z)) * 3 - (x.Item2.IsShadow ? 1 : 0) + (y.Item2.IsShadow ? 1 : 0),
                    CameraViewAngle.fourthQuadrant => (x.Item1.X - x.Item1.Y + x.Item1.Z - (y.Item1.X - y.Item1.Y + y.Item1.Z)) * 3 - (x.Item2.IsShadow ? 1 : 0) + (y.Item2.IsShadow ? 1 : 0),
                    _ => 0,
                };
            }
        }
    }
}
