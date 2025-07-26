using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;//可以省略 Math.

namespace _07GeodeticLine
{
    internal class Algo
    {
        double u1, u2, l, a1, a2, b1, b2;//辅助计算参数
        double A1, lambda, sigma, sigma_1, Sin_A0;//大地方位角计算参数
        double delta = 0.0;double delta0 = 0.0;
        public double S;//大地线长度计算参数

        //构造函数

        //辅助计算
        public void Cal_Help(DataCenter data)
        {
            u1 = Atan(Sqrt(1 - data.e2) * Tan(data.B1));
            u2 = Atan(Sqrt(1 - data.e2) * Tan(data.B2));
            l = data.L2 - data.L1;
            a1 = Sin(u1) * Sin(u2);
            a2 = Cos(u1) * Cos(u2);
            b1 = Cos(u1) * Sin(u2);
            b2 = Sin(u1) * Cos(u2);
        }

        //计算起点大地方位角
        public void Cal_A1(DataCenter data)
        {
            do
            {
                //计算 A1
                lambda = l + delta;
                double p = Cos(u2) * Sin(lambda);
                double q = b1 - b2 * Cos(lambda);

                A1 = Atan(p / q);
                if (p > 0)
                {
                    if (q > 0) A1 = Abs(A1);
                    else A1 = PI - Abs(A1);
                }
                else
                {
                    if (q < 0) A1 = PI + Abs(A1);
                    else A1 = 2 * PI - Abs(A1);
                }
                if (A1 < 0) A1 += 2 * PI;
                if (A1 > 2 * PI) A1 -= 2 * PI;

                //计算 sigma
                double Sin_sigma = p * Sin(A1) + q * Cos(A1);
                double Cos_sigma = a1 + a2 * Cos(lambda);

                sigma = Atan(Sin_sigma / Cos_sigma);
                if (Cos_sigma > 0) sigma = Abs(sigma);
                else sigma = PI - Abs(sigma);

                //计算 delta
                Sin_A0 = Cos(u1) * Sin(A1);
                sigma_1 = Atan(Tan(u1) / Cos(A1));
                double alpha = data.e2 / 2 + Pow(data.e2, 2) / 8 + Pow(data.e2, 3) / 16
                    - (Pow(data.e2, 2) / 16 + Pow(data.e2, 3) / 16) * (1 - Sin_A0 * Sin_A0)
                    + 3 * Pow(data.e2, 3) / 128 * Pow(1 - Sin_A0 * Sin_A0, 2);
                double beta = (Pow(data.e2, 2) / 16 + Pow(data.e2, 3) / 16) * (1 - Sin_A0 * Sin_A0)
                    - Pow(data.e2, 3) / 32 * Pow(1 - Sin_A0 * Sin_A0, 2);
                double gamma = Pow(data.e2, 3) / 256 * Pow(1 - Sin_A0 * Sin_A0, 2);

                delta = (alpha * sigma + beta * Cos(2 * sigma_1 + sigma) * Sin(sigma)
                    + gamma * Sin(2 * sigma) * Cos(4 * sigma_1 + 2 * sigma)) * Sin_A0;

                //迭代条件
                if (Abs(delta0 - delta) < 1e-10) break;
                 delta0 = delta;
            }
            while (true);            
        }

        //计算大地线长度 S 
        public void Cal_S(DataCenter data)
        {
            double k2 = data.e12 * (1 - Sin_A0 * Sin_A0);
            double k4 = k2 * k2;
            double k6 = k4 * k2;
            double A = (1 - k2 / 4 + 7 * k4 / 64 - 15 * k6 / 256) / data.b;
            double B = (k2 / 4 - k4 / 8 + 37 * k6 / 512);
            double C = (k4 / 128 - k6 / 128);
            double Xs = C * Sin(2 * sigma) * Cos(4 * sigma_1 + 2 * sigma);

            S = (sigma - B * Sin(sigma) * Cos(2 * sigma_1 + sigma) - Xs) / A;
        }
    }
}
