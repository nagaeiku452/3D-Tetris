using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tetris
{
    internal static class ColorMatrixGenerator
    {

        ///<summary>generate a <seealso cref="ColorMatrix"/> to change <seealso cref="Color.White"/> to <paramref name="endColor"/>. </summary>
        internal static void GenerateColorMatrix(Color endColor, ColorMatrix colorMatrix)
        {
            if (colorMatrix == null)
            {
                throw new NullReferenceException("ColorMarix is null!");
            }

            InitializeColorMatrix(colorMatrix);
            colorMatrix.Matrix00 = colorMatrix.Matrix10 = colorMatrix.Matrix20 = endColor.R / 768f;
            colorMatrix.Matrix01 = colorMatrix.Matrix11 = colorMatrix.Matrix21 = endColor.G / 768f;
            colorMatrix.Matrix02 = colorMatrix.Matrix12 = colorMatrix.Matrix22 = endColor.B / 768f;
            colorMatrix.Matrix33 = endColor.A / 256f;
        }

        private static void InitializeColorMatrix(ColorMatrix colorMatrix)
        {
            colorMatrix.Matrix00 = colorMatrix.Matrix11 = colorMatrix.Matrix22 = colorMatrix.Matrix33 = colorMatrix.Matrix44 = 1;
            colorMatrix.Matrix01 = colorMatrix.Matrix12 = colorMatrix.Matrix23 = colorMatrix.Matrix34 = colorMatrix.Matrix40 = 0;
            colorMatrix.Matrix02 = colorMatrix.Matrix13 = colorMatrix.Matrix24 = colorMatrix.Matrix30 = colorMatrix.Matrix41 = 0;
            colorMatrix.Matrix03 = colorMatrix.Matrix14 = colorMatrix.Matrix20 = colorMatrix.Matrix31 = colorMatrix.Matrix42 = 0;
            colorMatrix.Matrix04 = colorMatrix.Matrix10 = colorMatrix.Matrix21 = colorMatrix.Matrix32 = colorMatrix.Matrix43 = 0;
        }
    }
}
