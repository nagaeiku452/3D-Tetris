using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class CameraViewAngleExtension
    {
        public static CameraViewAngle CounterClockWisedShift(this CameraViewAngle curViewAngle)
        {
            return curViewAngle switch
            {
                CameraViewAngle.firstQuadrant => CameraViewAngle.secondQuadrant,
                CameraViewAngle.secondQuadrant => CameraViewAngle.thirdQuadrant,
                CameraViewAngle.thirdQuadrant => CameraViewAngle.fourthQuadrant,
                CameraViewAngle.fourthQuadrant => CameraViewAngle.firstQuadrant,
                _ => default,
            };
        }

        public static CameraViewAngle ClockWisedShift(this CameraViewAngle curViewAngle)
        {
            return curViewAngle switch
            {
                CameraViewAngle.firstQuadrant => CameraViewAngle.fourthQuadrant,
                CameraViewAngle.secondQuadrant => CameraViewAngle.firstQuadrant,
                CameraViewAngle.thirdQuadrant => CameraViewAngle.secondQuadrant,
                CameraViewAngle.fourthQuadrant => CameraViewAngle.thirdQuadrant,
                _ => default,
            };
        }
    }
}
