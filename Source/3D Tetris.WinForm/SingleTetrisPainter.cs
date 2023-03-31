using _3D_Tetris.Drawing;
using MainGame.Numeric;
using MainGame.Physics.Blocking;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Drawing;

namespace _3D_Tetris.WinForm
{
    internal class SingleTetrisPainter
    {
        private readonly StaticGridDynamicWorld<TetrisBodyBase> singleTetrisWorld = new(new StaticGridCollisionDispatcher(new DefaultStaticGridCollisionAlgorithmConfiguration()), new BlockingPhysicsConfiguration<TetrisBodyBase>());
        private readonly TetrisBodyBase singleTetrisModel = new(StaticGridRigidBodyType.Static);
        private readonly WorldObjectPainter objectPainter;
        private readonly SingleModelBoxShader shader = new();

        public SingleTetrisPainter(BWBoxSprite boxSprite)
        {
            objectPainter = new WorldObjectPainter(boxSprite, new WorldViewBox(Vector3i.Zero, Vector3i.Zero));
        }

        internal void PaintSingleTetris(Graphics g, System.Drawing.Point anchor, GridCollisionShape shape, System.Drawing.Color color, bool IsHeld = false)
        {
            singleTetrisModel.GridCollisionShape = shape;
            singleTetrisModel.PaintColor = color;
            singleTetrisModel.WorldTransform = Vector3i.Zero;
            singleTetrisWorld.AddCollisionObject(singleTetrisModel);

            objectPainter.ViewBoxMin = GetTrueMin(singleTetrisModel.MinPoint, objectPainter.ViewBoxMin);
            objectPainter.ViewBoxMax = GetTrueMax(singleTetrisModel.MaxPoint, objectPainter.ViewBoxMax);
            shader.AddAdditionInfo(IsHeld, "Held");
            objectPainter.PaintWorld(singleTetrisWorld, g, anchor, CameraViewAngle.firstQuadrant, shader);

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
