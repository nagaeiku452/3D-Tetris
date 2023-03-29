using MainGame.Physics.StaticGridSystem;
using _3D_Tetris.Drawing;
using MainGame.Physics.Blocking;

namespace _3D_Tetris
{
    public class TetrisBodyBase : BlockingRigidBody
    {
        
        public virtual Color PaintColor { get; set; }

        public virtual bool IsShadow { get; set; }

        public TetrisBodyBase(StaticGridRigidBodyType bodyType) : base(bodyType)
        {

        }
    }
}
