using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class WorldViewPainter : IDisposable
    {
        private readonly WorldObjectPainter objectPainter;
        private readonly GroundPainter groundPainter;
        private readonly WorldViewBoxShader shader = new();

        //public Size WorldViewSize
        //{
        //    get
        //    {
        //        return groundPainter.GroundSize + new Size(0, Math.Abs(objectPainter.UnitZProjection.Y) * (objectPainter.ViewBoxMax.Z - objectPainter.ViewBoxMin.Z));
        //    }
        //}

        public Point OriginPosition
        {
            get
            {
                return new Point(Math.Abs(objectPainter.UnitYProjection.X) * (objectPainter.ViewBoxMax.Y - objectPainter.ViewBoxMin.Y + 1), Math.Abs(objectPainter.UnitZProjection.Y) * (objectPainter.ViewBoxMax.Z - objectPainter.ViewBoxMin.Z + 1));
            }
        }

        public WorldViewPainter(Vector3i viewBoxMin, Vector3i viewBoxMax, BWBoxSprite boxSprite, GroundPainter groundPainter)
        {
            objectPainter = new WorldObjectPainter(boxSprite, new WorldViewBox(viewBoxMin, viewBoxMax));
            this.groundPainter = groundPainter;
            //groundPainter.ConfigureProjection(new Point(unitBoxZSurfaceWidth / 2, unitBoxZSurfaceHeight / 2), new Point(-unitBoxZSurfaceWidth / 2, unitBoxZSurfaceHeight / 2), viewBoxMax.X - viewBoxMin.X + 1, viewBoxMax.Y - viewBoxMin.Y + 1);
        }

        //public void ChangeViewBoxSize(Vector3i min, Vector3i max)
        //{
        //    objectPainter.ChangeViewBoxSize(min, max);

        //}

        public void ResizeViewBox(Vector3i viewBoxMin, Vector3i viewBoxMax)
        {
            objectPainter.ViewBoxMin = viewBoxMin;
            objectPainter.ViewBoxMax = viewBoxMax;
        }

        public void PaintWorld<T>(StaticGridDynamicWorld<T> world, Image canvas, Point originPos, CameraViewAngle viewAngle = CameraViewAngle.firstQuadrant) where T : TetrisBodyBase
        {
            bool b = viewAngle == CameraViewAngle.secondQuadrant || viewAngle == CameraViewAngle.fourthQuadrant;

            shader.AddAdditionInfo(objectPainter.ViewBoxMin, "viewBoxMin");
            shader.AddAdditionInfo(objectPainter.ViewBoxMax, "viewBoxMax");
            groundPainter?.PaintGround(canvas, originPos - new Size(b ? (objectPainter.ViewBoxMax.Y - objectPainter.ViewBoxMin.Y + 1) * objectPainter.UnitYProjection.X * 2 : 0, 0), b);
            objectPainter.PaintWorld(world, canvas, originPos, viewAngle, shader);
        }

        public void Dispose()
        {
            ((IDisposable)objectPainter).Dispose();
            ((IDisposable)groundPainter)?.Dispose();
        }
    }
}
