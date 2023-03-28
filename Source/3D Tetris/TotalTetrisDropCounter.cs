using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    class TotalTetrisDropCounter
    {
        public int TotalTetrisDropped { get; private set; } = 0;
        public void Init()
        {
            TotalTetrisDropped = 0;
        }

        public void Dropped()
        {
            TotalTetrisDropped += 1;
        }
    }
}
