using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;

namespace _3D_Tetris
{
    internal readonly struct StaticGridRotation
    {
        public static StaticGridRotation Identity => new(StaticGridDirection.NoDirection);
        public static StaticGridRotation PositiveXRotation => new(StaticGridDirection.PositiveUnitX);
        public static StaticGridRotation PositiveYRotation => new(StaticGridDirection.PositiveUnitY);
        public static StaticGridRotation PositiveZRotation => new(StaticGridDirection.PositiveUnitZ);
        public static StaticGridRotation NegativeXRotation => new(StaticGridDirection.NegativeUnitX);
        public static StaticGridRotation NegativeYRotation => new(StaticGridDirection.NegativeUnitY);
        public static StaticGridRotation NegativeZRotation => new(StaticGridDirection.NegativeUnitZ);
        public StaticGridRotation Inverse => new(matrix.Transpose);

        private readonly Matrix3x3i matrix;

        public StaticGridRotation(StaticGridDirection dir)
        {
            matrix = GetRotateMatrix(dir);
        }

        private StaticGridRotation(Matrix3x3i m)
        {
            matrix = m;
        }

        public static bool operator ==(StaticGridRotation left, StaticGridRotation right)
        {
            return left.matrix == right.matrix;
        }
        public static bool operator !=(StaticGridRotation left, StaticGridRotation right)
        {
            return !(left == right);
        }

        private static Matrix3x3i GetRotateMatrix(StaticGridDirection dir)
        {
            return dir switch
            {
                StaticGridDirection.NoDirection => Matrix3x3i.Identity,
                //+y->+z,+z->-y
                StaticGridDirection.PositiveUnitX => new Matrix3x3i(1, 0, 0, 0, 0, -1, 0, 1, 0),
                //+y->-z,+z->+y
                StaticGridDirection.NegativeUnitX => new Matrix3x3i(1, 0, 0, 0, 0, 1, 0, -1, 0),
                //+x->+z,+z->-x
                StaticGridDirection.PositiveUnitY => new Matrix3x3i(0, 0, -1, 0, 1, 0, 1, 0, 0),
                //+x->-z,+z->+x
                StaticGridDirection.NegativeUnitY => new Matrix3x3i(0, 0, 1, 0, 1, 0, -1, 0, 0),
                //+x->+y,+y->-x
                StaticGridDirection.PositiveUnitZ => new Matrix3x3i(0, -1, 0, 1, 0, 0, 0, 0, 1),
                //+x->-y,+y->+x
                StaticGridDirection.NegativeUnitZ => new Matrix3x3i(0, 1, 0, -1, 0, 0, 0, 0, 1),
                _ => Matrix3x3i.Identity,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is StaticGridRotation r && this == r;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static StaticGridRotation operator *(StaticGridRotation left, StaticGridRotation right)
        {
            return new StaticGridRotation(left.matrix * right.matrix);
        }
        public static Vector3i operator *(StaticGridRotation left, Vector3i v)
        {
            return left.matrix * v;
        }

        public static StaticGridRotation operator ^(StaticGridRotation left, int power)
        {
            Matrix3x3i m = Matrix3x3i.Identity;
            for (int i = 0; i < power; i++)
            {
                m *= power >= 0 ? left.matrix : left.matrix.Transpose;
            }
            return new(m);
        }
    }
}
