using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _09Section_Cal
{
    internal class Algo
    {
        public List<Point> Z_Points;//纵断面内插点
        public double Ver_Len, Ver_S;//纵断面长度，面积

        public List<Point> N1_Points;//横断面1内插点
        public List<Point> N2_Points;//横断面2内插点
        public double Hor1_S;//横断面1面积
        public double Hor2_S;//横断面2面积

        //构造函数
        public Algo()
        {
            Z_Points = new List<Point>();
            N1_Points = new List<Point>();
            N2_Points = new List<Point>();
        }
        //方位角计算
        public double Cal_A(Point a, Point b)
        {
            double dy = b.Y - a.Y;
            double dx = b.X - a.X;
            double A = Atan(Abs(dy / dx));//要加绝对值
            if (dx == 0)
            {
                if (dy > 0) A = PI;
                else A = 3 * PI / 2;
            }
            else if (dx > 0)
            {
                if (dy < 0) A = 2 * PI - A;
            }
            else
            {
                if (dy < 0) A = PI + A;
                else A = PI - A;
            }
            return A;
        }
        //两点间距离计算
        public double Cal_D(Point a, Point b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double D = Sqrt(dx * dx + dy * dy);
            return D;
        }
        //反距离加权内插高程
        public double Cal_H(Point a,DataCenter data)
        {
            a.Closest_Points = data.KP_Points.OrderBy(p => Cal_D(p, a)).Take(20).ToList();
            double up = 0.0;double down = 0.0;
            foreach(Point p in a.Closest_Points)
            {
                up += p.H / Cal_D(p, a);
                down += 1 / Cal_D(p, a);
            }
            //double up = a.Closest_Points.Sum(p => p.H / Cal_D(p, a));
            //double down = a.Closest_Points.Sum(p => 1 / Cal_D(p, a));
            double h = up / down;
            return h;
        }
        //纵断面计算
        public void Cal_Vertical(DataCenter data)
        {
            double L01 = Cal_D(data.K_Points[0], data.K_Points[1]);
            double L12 = Cal_D(data.K_Points[1], data.K_Points[2]);
            Ver_Len = L01 + L12;

            double l = 10.0;
            double Li = l;
            int i = 1;
            Z_Points.Add(data.K_Points[0]);
            //插值点 Zi 在 K0K1 直线上
            do
            {
                Point p = new Point();
                double alpha_01 = Cal_A(data.K_Points[0], data.K_Points[1]);
                p.X = data.K_Points[0].X + Li * Cos(alpha_01);
                p.Y = data.K_Points[0].Y + Li * Sin(alpha_01);
                p.Name = $"Z{i}";
                p.H = Cal_H(p, data);
                Z_Points.Add(p);                
                i++;
                Li += l;
            }
            while (Li < L01);
            Z_Points.Add(data.K_Points[1]);
            //插值点 Zi 在 K1K2 直线上
            do
            {
                Point p = new Point();
                double alpha_12 = Cal_A(data.K_Points[1], data.K_Points[2]);
                p.X = data.K_Points[1].X + (Li - L01) * Cos(alpha_12);
                p.Y = data.K_Points[1].Y + (Li - L01) * Sin(alpha_12);
                p.Name = $"Z{i}";
                p.H = Cal_H(p, data);
                Z_Points.Add(p);
                i++;
                Li += l;
            }
            while (Li < Ver_Len);
            Z_Points.Add(data.K_Points[2]);
            //纵断面面积计算
            Ver_S = 0.0;
            for (int j = 0; j < Z_Points.Count - 1; j++)
            {
                Ver_S += (Z_Points[j].H + Z_Points[j + 1].H - 2 * data.H0) 
                    * Cal_D(Z_Points[j], Z_Points[j + 1]) / 2;
            }
        }
        //横断面1计算
        public void Cal_Horizontal1(DataCenter data)
        {
            data.M_Points[0].H = Cal_H(data.M_Points[0], data);//先计算 M 点的高程

            double alpha_M = Cal_A(data.K_Points[0], data.K_Points[1]) + PI / 2;
            double Li = 0.0;
            int j = 1;
            //进行内插点
            for(int i = -5; i <= 5; i++)
            {
                Li = 5 * i;
                Point p = new Point();                
                p.X = data.M_Points[0].X + Li * Cos(alpha_M);
                p.Y = data.M_Points[0].Y + Li * Sin(alpha_M);
                p.Name = $"Z{j}";
                p.H = Cal_H(p, data);                
                if(i == 0)
                {
                    N1_Points.Add(data.M_Points[0]);
                    continue;
                }
                N1_Points.Add(p);
                j++;
            }
            //计算横断面面积
            Hor1_S = 0.0;
            for (int k = 0; k < N1_Points.Count - 1; k++)
            {
                Hor1_S += (N1_Points[k].H + N1_Points[k + 1].H - 2 * data.H0)
                    * Cal_D(N1_Points[k], N1_Points[k + 1]) / 2;
            }
        }
        //横断面2计算
        public void Cal_Horizontal2(DataCenter data)
        {
            data.M_Points[1].H = Cal_H(data.M_Points[1], data);//先计算 M 点的高程

            double alpha_M = Cal_A(data.K_Points[1], data.K_Points[2]) + PI / 2;
            double Li = 0.0;
            int j = 1;
            //进行内插点
            for (int i = -5; i <= 5; i++)
            {
                Li = 5 * i;
                Point p = new Point();
                p.X = data.M_Points[1].X + Li * Cos(alpha_M);
                p.Y = data.M_Points[1].Y + Li * Sin(alpha_M);
                p.Name = $"Z{j}";
                p.H = Cal_H(p, data);
                if (i == 0)
                {
                    N2_Points.Add(data.M_Points[1]);
                    continue;
                }
                N2_Points.Add(p);
                j++;
            }
            //计算横断面面积
            Hor1_S = 0.0;
            for (int k = 0; k < N2_Points.Count - 1; k++)
            {
                Hor2_S += (N2_Points[k].H + N2_Points[k + 1].H - 2 * data.H0)
                    * Cal_D(N2_Points[k], N2_Points[k + 1]) / 2;
            }
        }
    }
}
