using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System.Collections.Generic;
using _3D_Tetris.Drawing;

namespace _3D_Tetris
{
    internal static class TetraCubeShapePrototypeProvider
    {
        private static readonly List<(GridCollisionShape, Color)> tetraCubeShapes;
        public static IReadOnlyList<(GridCollisionShape, Color)> TetraCubeShapes => tetraCubeShapes;

        static TetraCubeShapePrototypeProvider()
        {
            tetraCubeShapes = new List<(GridCollisionShape, Color)>();

            GridCollisionShape ITetraCubeShape = new GridBoxCollisionShape(-Vector3i.UnitX, Vector3i.UnitX * 2);
            tetraCubeShapes.Add((ITetraCubeShape, Color.FromRgb(0, 255, 255)));

            GridCollisionShape OTetraCubeShape = new GridBoxCollisionShape(Vector3i.Zero, new Vector3i(1, 1, 0));
            tetraCubeShapes.Add((OTetraCubeShape, Color.FromRgb(255, 215, 0)));

            MultiGridCollisionShape TTetraCubeShape = new();
            TTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.Zero), Vector3i.UnitY);
            TTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(-Vector3i.UnitX, Vector3i.UnitX), Vector3i.Zero);
            tetraCubeShapes.Add((TTetraCubeShape, Color.FromRgb(148, 0, 211)));

            MultiGridCollisionShape LTetraCubeShape = new();
            LTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(-Vector3i.UnitX, Vector3i.UnitX), Vector3i.Zero);
            LTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.Zero), new Vector3i(1, 1, 0));
            tetraCubeShapes.Add((LTetraCubeShape, Color.FromRgb(0, 0, 255)));

            MultiGridCollisionShape STetraCubeShape = new();
            STetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitX), Vector3i.Zero);
            STetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitX), new Vector3i(-1, 1, 0));
            tetraCubeShapes.Add((STetraCubeShape, Color.FromRgb(87, 255, 0)));

            MultiGridCollisionShape DTetraCubeShape = new();
            DTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitX), Vector3i.Zero);
            DTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitZ), Vector3i.UnitY);
            tetraCubeShapes.Add((DTetraCubeShape, Color.FromRgb(255, 0, 0)));

            MultiGridCollisionShape FTetraCubeShape = new();
            FTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitY), Vector3i.Zero);
            FTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitZ), Vector3i.UnitX);
            tetraCubeShapes.Add((FTetraCubeShape, Color.FromRgb(255, 165, 0)));

            MultiGridCollisionShape BTetraCubeShape = new();
            BTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.UnitX), Vector3i.Zero);
            BTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.Zero), Vector3i.UnitY);
            BTetraCubeShape.AddLeafShape(new GridBoxCollisionShape(Vector3i.Zero, Vector3i.Zero), Vector3i.UnitZ);
            tetraCubeShapes.Add((BTetraCubeShape, Color.FromRgb(255, 255, 255)));
        }
    }
}
