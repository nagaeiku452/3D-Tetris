using MainGame.Numeric;

namespace _3D_Tetris
{
    public static class CameraViewAngleExtension
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

        public static Matrix3x3i GenerateRotationMatrix(this CameraViewAngle viewAngle)
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
