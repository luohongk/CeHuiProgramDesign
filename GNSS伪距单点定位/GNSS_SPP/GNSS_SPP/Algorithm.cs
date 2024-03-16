using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS_SPP
{
    class Algorithm
    {
        public static void caaulate(ref DataCenter dc, ref TextBox texBox1, ref TextBox texBox2, ref TextBox texBox3)
        {
            int Posline = 0;
            for (int k = 0; k < dc.positions.Count; k = Posline)
            {
                //伪距单点定位计算
                //定义光速
                double c = 3 * Math.Pow(10, -8);
                double x0, y0, z0;
                double[,] x = new double[4, 1];
                Matrix matrix_x = new Matrix(x);
                x0 = Convert.ToDouble(texBox1.Text);
                y0 = Convert.ToDouble(texBox2.Text);
                z0 = Convert.ToDouble(texBox3.Text);

                int m = dc.positions[Posline].SatNum;
                double[,] B = new double[m, 4];
                double[,] L = new double[m, 1];
                //此处差一个迭代收敛
                for (int i = 0; i < m; ++i)
                {
                    double P0 = Math.Sqrt(Math.Pow(x0 - dc.positions[i].x, 2) + Math.Pow(y0 - dc.positions[i].y, 2) + Math.Pow(z0 - dc.positions[i].z, 2));
                    B[i, 0] = -1 * (dc.positions[Posline].x - x0) / P0;
                    B[i, 1] = -1 * (dc.positions[Posline].y - y0) / P0;
                    B[i, 2] = -1 * (dc.positions[Posline].z - z0) / P0;
                    B[i, 3] = -1 * c;
                    L[i, 0] = dc.positions[i].SatClock + dc.positions[i].L - P0 - dc.positions[i].Delay;
                    Posline++;
                }

                Matrix matrixB = new Matrix(B);
                Matrix matrixL = new Matrix(L);
                matrix_x = (matrixB.Transpose() * matrixB).Inverse() * (matrixB.Transpose() * matrixL);
                x0 = x0 + matrix_x[0, 0];
                y0 = y0 + matrix_x[1, 0];
                z0 = z0 + matrix_x[2, 0];

                truePosition truePosition = new truePosition();
                truePosition.x = x0;
                truePosition.y = y0;
                truePosition.z = z0;
                truePosition.LiYuan = dc.positions[Posline - 1].GPS_Time;
                dc.truePositions.Add(truePosition);
                
            }
        }
    }
}
