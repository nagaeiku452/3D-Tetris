using MainGame.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class CameraViewRotationMatrix
    {
        public static Matrix3x3i GenerateRotationMatrix(CameraViewAngle viewAngle)
        {
            return viewAngle switch
            {
                CameraViewAngle.firstQuadrant => Matrix3x3i.Identity,
                CameraViewAngle.secondQuadrant => new Matrix3x3i(0, 1, 0, -1, 0, 0, 0, 0, 1),
                CameraViewAngle.thirdQuadrant => new Matrix3x3i(-1, 0, 0, 0, -1, 0, 0, 0, 1),
                CameraViewAngle.fourthQuadrant => new Matrix3x3i(0, -1, 0, 1, 0, 0, 0, 0, 1),
                _ => Matrix3x3i.Identity,
            };
        }
    }
}
