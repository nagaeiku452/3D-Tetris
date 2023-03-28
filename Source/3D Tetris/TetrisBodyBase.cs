using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainGame.Physics.Blocking;
using MainGame.Numeric;
using System.Diagnostics;

namespace _3D_Tetris
{
    internal class TetrisBodyBase : BlockingRigidBody
    {
        
        public virtual Color PaintColor { get; set; }

        public virtual bool IsShadow { get; set; }

        public TetrisBodyBase(StaticGridRigidBodyType bodyType) : base(bodyType)
        {

        }

    }
}
