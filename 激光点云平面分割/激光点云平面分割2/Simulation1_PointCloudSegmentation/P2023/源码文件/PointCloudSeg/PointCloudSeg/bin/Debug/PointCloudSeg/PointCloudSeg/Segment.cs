using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointCloudSeg
{
    public class Segment//分割计算帮助类
    {

        /// <summary>
        /// 计算两点三维距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDist(Pointinfo p1, Pointinfo p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
        }

        /// <summary>
        /// 计算三角形面积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static double GetArea(Pointinfo p1,Pointinfo p2,Pointinfo p3)
        {
            double a = GetDist(p1, p2);
            double b = GetDist(p2, p3);
            double c = GetDist(p3, p1);

            double p = (a + b + c) / 2.0;
            double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            if (Math.Abs(S) < 0.1)
            {
                MessageBox.Show("三点共线！");
                return -1;
            }

            return S;
        }

        /// <summary>
        /// 计算平面系数
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static double[] GetABCD(Pointinfo p1, Pointinfo p2, Pointinfo p3)
        {
            double[] ABCD = new double[4];
            ABCD[0] = (p2.Y - p1.Y) * (p3.Z - p1.Z) - (p3.Y - p1.Y) * (p2.Z - p1.Z);
            ABCD[1] = (p2.Z - p1.Z) * (p3.X - p1.X) - (p3.Z - p1.Z) * (p2.X - p1.X);
            ABCD[2] = (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
            ABCD[3] = -ABCD[0] * p1.X - ABCD[1] * p1.Y - ABCD[2] * p1.Z;
            return ABCD;
        }

        /// <summary>
        /// 计算点到平面距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="ABCD"></param>
        /// <returns></returns>
        public static double GetP2S(Pointinfo p1, double[] ABCD)
        {
            double up = Math.Abs(ABCD[0] * p1.X + ABCD[1] * p1.Y + ABCD[2] * p1.Z + ABCD[3]);
            double down = Math.Sqrt(Math.Pow(ABCD[0], 2) + Math.Pow(ABCD[1], 2) + Math.Pow(ABCD[2], 2));

            return up / down;
        }

        /// <summary>
        /// 计算投影坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="ABCD"></param>
        /// <returns></returns>
        public static double[] GetProject(Pointinfo p1, double[] ABCD)
        {
            double[] xyz=new double[3];
            double A = ABCD[0], B = ABCD[1], C = ABCD[2], D = ABCD[3];
            double down = Math.Pow(ABCD[0], 2) + Math.Pow(ABCD[1], 2) + Math.Pow(ABCD[2], 2);

            xyz[0] = ((B * B + C * C) * p1.X - A * (B * p1.Y + C * p1.Z + D)) / down;
            xyz[1] = ((A * A + C * C) * p1.Y - B * (A * p1.X + C * p1.Z + D)) / down;
            xyz[2] = ((A * A + B * B) * p1.Z - C * (A * p1.X + B * p1.Y + D)) / down;
            return xyz;
        }
    }
}
