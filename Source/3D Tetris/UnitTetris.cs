using MainGame.Physics.Utilities;
using MainGame.Physics.StaticGridSystem;
using MainGame.Numeric;
using _3D_Tetris.Drawing;

namespace _3D_Tetris
{
    internal class UnitTetris : TetrisBodyBase, IInstancePrototype
    {
        private static readonly GridBoxCollisionShape UnitTetrisShape = new(Vector3i.Zero, Vector3i.Zero);

        public UnitTetris() : base(StaticGridRigidBodyType.Static)
        {
            SetActivationState(GridBodyActivationState.Sleep);
            GridCollisionShape = UnitTetrisShape;
            AffectByGravity = false;
        }

        public void Clear()
        {
            SetActivationState(GridBodyActivationState.Sleep);
            GridCollisionShape = UnitTetrisShape;
            AffectByGravity = false;
            WorldTransform = new();
            PaintColor = Color.Transparent;
        }
    }
}
