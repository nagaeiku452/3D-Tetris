using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal abstract class BoxShader
    {
        public abstract void AddAdditionInfo<T>(T info, string tag);
        public abstract Color ColorAdjust(Color oriColor);
    }
}
