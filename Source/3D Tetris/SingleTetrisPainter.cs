using MainGame.Numeric;
using MainGame.Physics.Blocking;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class SingleTetrisPainter
    {
        private readonly StaticGridDynamicWorld<TetrisBodyBase> singleTetrisWorld = new(new StaticGridCollisionDispatcher(new DefaultStaticGridCollisionAlgorithmConfiguration()), new BlockingPhysicsConfiguration<TetrisBodyBase>());
        private readonly RotatableTetris singleTetrisModel = new();
        private readonly WorldObjectPainter objectPainter;
        private readonly SingleModelBoxShader shader = new();

        public SingleTetrisPainter(BWBoxSprite boxSprite)
        {
            objectPainter = new WorldObjectPainter(boxSprite, new WorldViewBox(Vector3i.Zero, Vector3i.Zero));
        }

        internal void PaintSingleTetris(Image backImage, Point anchor, GridCollisionShape shape, Color color, bool IsHeld = false)
        {
            singleTetrisModel.GridCollisionShape = shape;
            singleTetrisModel.PaintColor = color;
            singleTetrisModel.WorldTransform = Vector3i.Zero;
            singleTetrisWorld.AddCollisionObject(singleTetrisModel);

            objectPainter.ViewBoxMin = GetTrueMin(singleTetrisModel.MinPoint, objectPainter.ViewBoxMin);
            objectPainter.ViewBoxMax = GetTrueMax(singleTetrisModel.MaxPoint, objectPainter.ViewBoxMax);
            shader.AddAdditionInfo(IsHeld, "Held");
            objectPainter.PaintWorld(singleTetrisWorld, backImage, anchor, CameraViewAngle.firstQuadrant, shader);

            singleTetrisWorld.RemoveCollisionObject(singleTetrisModel);
        }

        private static Vector3i GetTrueMin(Vector3i viewBoxMin, Vector3i viewBoxMax)
        {
            return new Vector3i(Math.Min(viewBoxMin.X, viewBoxMax.X), Math.Min(viewBoxMin.Y, viewBoxMax.Y), Math.Min(viewBoxMin.Z, viewBoxMax.Z));
        }
        private static Vector3i GetTrueMax(Vector3i viewBoxMin, Vector3i viewBoxMax)
        {
            return new Vector3i(Math.Max(viewBoxMin.X, viewBoxMax.X), Math.Max(viewBoxMin.Y, viewBoxMax.Y), Math.Max(viewBoxMin.Z, viewBoxMax.Z));
        }
    }
}
