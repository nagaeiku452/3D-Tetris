using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal class WorldViewBox
    {
        private readonly Dictionary<Vector3i, TetrisBodyBase> paintData;
        private Vector3i max;
        private Vector3i min;
        public Vector3i Min
        {
            get
            {
                return min;
            }
            set
            {
                min = new Vector3i(Math.Min(max.X, value.X), Math.Min(max.Y, value.Y), Math.Min(max.Z, value.Z));
                max = new Vector3i(Math.Max(max.X, value.X), Math.Max(max.Y, value.Y), Math.Max(max.Z, value.Z));
            }
        }
        public Vector3i Max
        {
            get => max;
            set
            {
                max = new Vector3i(Math.Max(min.X, value.X), Math.Max(min.Y, value.Y), Math.Max(min.Z, value.Z));
                min = new Vector3i(Math.Min(min.X, value.X), Math.Min(min.Y, value.Y), Math.Min(min.Z, value.Z));
            }
        }


        public WorldViewBox(Vector3i min, Vector3i max)
        {
            Min = min;
            Max = max;
            paintData = new Dictionary<Vector3i, TetrisBodyBase>();
        }

        public IEnumerable<KeyValuePair<Vector3i, TetrisBodyBase>> WorldViewInterception<T>(StaticGridDynamicWorld<T> world) where T : TetrisBodyBase
        {
            paintData.Clear();
            foreach (T unitTetrisBody in world.GridCollisionObjects)
            {
                if(unitTetrisBody.GridCollisionShape is null)
                {
                    continue;
                }

                foreach ((SimpleGridCollisionShape, Vector3i) shapePositionPair in unitTetrisBody.GridCollisionShape)
                {
                    Vector3i min = shapePositionPair.Item1.MinPoint;
                    Vector3i max = shapePositionPair.Item1.MaxPoint;
                    for (int i = 0; i < max.X - min.X + 1; i++)
                    {
                        for (int j = 0; j < max.Y - min.Y + 1; j++)
                        {
                            for (int k = 0; k < max.Z - min.Z + 1; k++)
                            {
                                Vector3i paintDataPos = unitTetrisBody.WorldTransform + shapePositionPair.Item2 + new Vector3i(i, j, k) + min;
                                if(paintDataPos.X >= Min.X && paintDataPos.Y >= Min.Y && paintDataPos.Z >= Min.Z && paintDataPos.X <= Max.X && paintDataPos.Y <= Max.Y && paintDataPos.Z <= Max.Z)
                                {
                                    paintData.TryAdd(paintDataPos, unitTetrisBody);
                                }
                            }
                        }
                    }
                }
            }
            foreach (KeyValuePair<Vector3i, TetrisBodyBase> item in paintData)
            {
                yield return item;
            }
        }

        //private Vector3i CaculateCameraAngleRelatedTransform(CameraViewAngle curAngle,Vector3i oriTransform)
        //{
        //    switch (curAngle)
        //    {
        //        case CameraViewAngle.firstQuadrant:
        //            return oriTransform;
        //        case CameraViewAngle.secondQuadrant:
        //            return new Vector3i(oriTransform.Y,-oriTransform.X);
        //        case CameraViewAngle.thirdQuadrant:
        //            break;
        //        case CameraViewAngle.fourthQuadrant:
        //            break;
        //        default:
        //            return oriTransform;
        //    }
        //}
    }
}
