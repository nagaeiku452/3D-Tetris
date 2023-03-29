using _3D_Tetris.Drawing;
using MainGame.Physics.StaticGridSystem;
using System.Collections.Generic;

namespace _3D_Tetris
{
    internal class NextTetrisQueue
    {
        private readonly IThreeDimTetrisSet threeDimTetrisSet = new TetraCubeTetrisSet() { TetraCubeSetLoop = true };
        //private readonly IThreeDimTetrisSet threeDimTetrisSet = new ITetraCubeTetrisSet();

        private readonly Queue<int> nextTetrisQueue = new();
        private int queueSize;

        public int QueueSize
        {
            get => queueSize;
            set
            {
                if (value >= 0)
                {
                    int max = value - nextTetrisQueue.Count;
                    for (int i = 0; i < max; i++)
                    {
                        nextTetrisQueue.Enqueue(GenerateData());
                    }
                    queueSize = value;
                }
            }
        }

        public int NextTetrisData
        {
            get
            {
                nextTetrisQueue.Enqueue(GenerateData());
                return nextTetrisQueue.Dequeue();
            }
        }

        public IEnumerable<(GridCollisionShape, Color)> EnumerateNextTetrisData
        {
            get
            {
                int i = queueSize;
                foreach (int item in nextTetrisQueue)
                {
                    yield return threeDimTetrisSet.GetTetrisData(item);
                    i--;
                    if (i == 0) { break; }
                }
            }
        }

        public (GridCollisionShape, Color) GetTetrisData(int num)
        {
            return threeDimTetrisSet.GetTetrisData(num);
        }

        private int GenerateData()
        {
            return threeDimTetrisSet.NextTetrisNum;
        }

        public NextTetrisQueue()
        {
            //QueueSize = 1;
        }

        private class ITetraCubeTetrisSet : IThreeDimTetrisSet
        {
            private static (GridCollisionShape, Color) nextTetrisData;

            public int NextTetrisNum => 0;

            static ITetraCubeTetrisSet()
            {
                nextTetrisData = (TetraCubeShapePrototypeProvider.TetraCubeShapes[0].Item1.CloneShape(), TetraCubeShapePrototypeProvider.TetraCubeShapes[0].Item2);
            }
            public int MaximumTetrisSize => 4;

            public int TotalTetrisVaries => 1;

            public (GridCollisionShape, Color) GetTetrisData(int num)
            {
                return num == 0 ? nextTetrisData : default;
            }
        }

    }
}
