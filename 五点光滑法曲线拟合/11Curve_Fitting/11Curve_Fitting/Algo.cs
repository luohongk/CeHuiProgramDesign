using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _11Curve_Fitting
{
    internal class Algo
    {
        //补充点（闭合）
        public void Add_Points_1(List<Point> points)
        {
            int n = points.Count;// 14

            Point pn_1 = new Point(-2, points[n - 2].X, points[n - 2].Y);
            Point pn = new Point(-1, points[n - 1].X, points[n - 1].Y);
            Point p1 = new Point(n , points[0].X, points[0].Y);
            Point p2 = new Point(n + 1, points[1].X, points[1].Y);

            points.Insert(0, pn_1); points.Insert(1, pn);
            points.Insert(n + 2, p1); points.Insert(n + 3, p2);            
        }
        //补充点（不闭合）  注：与书上公式不太一样,原理不太懂
        public void Add_Points_2(List<Point> points)
        {
            double xa = points[2].X - 3 * points[1].X + 3 * points[0].X;
            double ya = points[2].Y - 3 * points[1].Y + 3 * points[0].Y;
            Point pa = new Point(-1, xa, ya);

            double xb = points[1].X - 3 * points[0].X + 3 * pa.X;
            double yb = points[1].Y - 3 * points[0].Y + 3 * pa.Y;
            Point pb = new Point(-2, xb, yb);

            int n = points.Count;

            double xc = points[n-3].X - 3 * points[n-2].X + 3 * points[n-1].X;
            double yc = points[n-3].X - 3 * points[n-2].X + 3 * points[n-1].X;
            Point pc = new Point(n, xc, yc);

            double xd = points[n - 2].X - 3 * points[n - 1].X + 3 * pc.X;
            double yd = points[n - 2].Y - 3 * points[n - 1].Y + 3 * pc.Y;
            Point pd = new Point(n + 1, xd, yd);

            points.Insert(0, pb); points.Insert(1, pa);
            points.Insert(n + 2, pc); points.Insert(n + 3, pd);
        }

        //计算曲线参数
        public void Cal_Curve(List<Point> points)
        {
            //计算准备
            for(int i = 2; i < points.Count - 2; i++)
            {                
                double a1 = points[i - 1].X - points[i - 2].X;
                double b1 = points[i - 1].Y - points[i - 2].Y;
                double a2 = points[i].X - points[i - 1].X;
                double b2 = points[i].Y - points[i - 1].Y;
                double a3 = points[i + 1].X - points[i].X;
                double b3 = points[i + 1].Y - points[i].Y;
                double a4 = points[i + 2].X - points[i + 1].X;
                double b4 = points[i + 2].Y - points[i + 1].Y;

                double w2 = Abs(a3 * b4 - a4 * b3);
                double w3 = Abs(a1 * b2 - a2 * b1);

                double a0 = w2 * a2 + w3 * a3;
                double b0 = w2 * b2 + w3 * b3;
                
                points[i].Cos_theta = a0 / Sqrt(a0 * a0 + b0 * b0);
                points[i].Sin_theta = b0 / Sqrt(a0 * a0 + b0 * b0);                            
            }
            //曲线参数计算
            for (int i = 2; i < points.Count - 2; i++)
            {
                double r = Sqrt(Pow(points[i + 1].X - points[i].X, 2) + Pow(points[i + 1].Y - points[i].Y, 2));

                points[i].E0 = points[i].X;
                points[i].E1 = r * points[i].Cos_theta;
                points[i].E2 = 3 * (points[i + 1].X - points[i].X) - r * (points[i + 1].Cos_theta + 2 * points[i].Cos_theta);
                points[i].E3 = -2 * (points[i + 1].X - points[i].X) + r * (points[i + 1].Cos_theta + points[i].Cos_theta);

                points[i].F0 = points[i].Y;
                points[i].F1 = r * points[i].Sin_theta;
                points[i].F2 = 3 * (points[i + 1].Y - points[i].Y) - r * (points[i + 1].Sin_theta + 2 * points[i].Sin_theta);
                points[i].F3 = -2 * (points[i + 1].Y - points[i].Y) + r * (points[i + 1].Sin_theta + points[i].Sin_theta);
            }
        }
    }
}
