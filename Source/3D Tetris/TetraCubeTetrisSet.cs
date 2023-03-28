using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class TetraCubeTetrisSet : IThreeDimTetrisSet
    {
        private static readonly IReadOnlyList<(GridCollisionShape, Color)> tetraCubeShapes = TetraCubeShapePrototypeProvider.TetraCubeShapes;
        private readonly Random r = new(((int)DateTime.Now.Ticks));
        private readonly List<int> permutationList = new();
        private readonly Queue<int> permutationQueue = new();
        private int lastQueueElement;
        private bool tetraCubeSetLoop;

        //set this to true will let choose be loop-like
        //means if one tetra tetris has already appeared twice, then all other tetris must've appeared once
        public bool TetraCubeSetLoop
        {
            get => tetraCubeSetLoop;
            set
            {
                if (value == false && tetraCubeSetLoop == true)
                {
                    permutationQueue.Clear();
                }
                tetraCubeSetLoop = value;
            }
        }
        public int NextTetrisNum => GenerateNextTetrisData();

        public int MaximumTetrisSize => 4;

        public int TotalTetrisVaries => tetraCubeShapes.Count;

        public (GridCollisionShape, Color) GetTetrisData(int num)
        {
            return (-1 < num && num < tetraCubeShapes.Count) ? tetraCubeShapes[num] : default;
        }

        private int GenerateNextTetrisData()
        {
            if (tetraCubeSetLoop)
            {
                if (permutationQueue.Count == 0)
                {
                    GenerateRandomPermutation();
                }

                return permutationQueue.Dequeue();
            }

            return r.Next(0, tetraCubeShapes.Count);
        }

        private void GenerateRandomPermutation()
        {
            permutationQueue.Clear();

            permutationList.Clear();
            for (int i = 0; i < tetraCubeShapes.Count; i++)
            {
                permutationList.Add(i);
            }
            bool IsFirst = true;

            while (permutationList.Count > 0)
            {
                int next = r.Next(0, permutationList.Count);
                if (IsFirst)
                {
                    next = r.Next(0, permutationList.Count - 1);
                    next += next >= lastQueueElement ? 1 : 0;
                    IsFirst = false;
                }
                permutationQueue.Enqueue(permutationList[next]);
                lastQueueElement = permutationList[next];
                permutationList.RemoveAt(next);
            }
        }
    }
}
