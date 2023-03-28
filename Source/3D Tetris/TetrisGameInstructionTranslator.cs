using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class TetrisGameInstructionTranslator<Key> where Key : struct, Enum
    {

        public static Key key { get; }
        static TetrisGameInstructionTranslator()
        {
        }
    }
}
