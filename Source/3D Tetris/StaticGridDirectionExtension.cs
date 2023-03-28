using MainGame.Numeric;
using MainGame.Physics.StaticGridSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class StaticGridDirectionExtension
    {
        internal static StaticGridDirection CrossProductWith(this StaticGridDirection direction, StaticGridDirection other)
        {
            Vector3i v = Vector3i.CrossProduct(direction.ToTransform(), other.ToTransform());
            if (!TryConvertToStaticGridDirection(v, out StaticGridDirection dir))
            {
                Debug.Fail("ConvertFail");
                return default;
            }
            return dir;
        }

        private static bool TryConvertToStaticGridDirection(Vector3i v, out StaticGridDirection direction)
        {
            bool b = true;
            if (v == Vector3i.Zero)
            {
                direction = StaticGridDirection.NoDirection;
            }
            else if (v == Vector3i.UnitX)
            {
                direction = StaticGridDirection.PositiveUnitX;
            }
            else if (v == -Vector3i.UnitX)
            {
                direction = StaticGridDirection.NegativeUnitX;
            }
            else if (v == Vector3i.UnitY)
            {
                direction = StaticGridDirection.PositiveUnitY;
            }
            else if (v == -Vector3i.UnitY)
            {
                direction = StaticGridDirection.NegativeUnitY;
            }
            else if (v == -Vector3i.UnitZ)
            {
                direction = StaticGridDirection.NegativeUnitZ;
            }
            else if (v == Vector3i.UnitZ)
            {
                direction = StaticGridDirection.PositiveUnitZ;
            }
            else
            {
                direction = StaticGridDirection.NoDirection;
                b = false;
            }
            return b;
        }
    }
}
