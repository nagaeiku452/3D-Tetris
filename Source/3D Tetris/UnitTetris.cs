using MainGame.Physics.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainGame.Physics.StaticGridSystem;
using MainGame.Numeric;
using System.Drawing;

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
