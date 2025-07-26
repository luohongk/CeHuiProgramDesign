using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Simulation1_PointCloudSegmentation
{
    class Algorithm
    {
        public static string ToGrid(DataCenter data)
        {
            string text = null;

            foreach(Point p in data.Points)
            {
                p.i = Convert.ToInt32(Floor(p.Y / 10.0));
                p.j = Convert.ToInt32(Floor(p.X / 10.0));
            }

            List<Point> C_Points = new List<Point>();
            foreach(Point p in data.Points)
            {
                if (p.i == 3 && p.j == 2) C_Points.Add(p);
            }
            double C_Zaver = C_Points.Sum(p => p.Z) / C_Points.Count;
            double C_Zmin = C_Points.OrderBy(p => p.Z).First().Z;
            double C_Zmax = C_Points.OrderBy(p => p.Z).Last().Z;
            double C_Zdiff = C_Zmax - C_Zmin;
            double C_Sigma2 = C_Points.Sum(p => Pow(p.Z - C_Zaver, 2)) / C_Points.Count;

            text += $"{data.Num}，P5 点的所在栅格的行 i，{data.Points[4].i}\r\n"; data.Num++;
            text += $"{data.Num}，P5 点的所在栅格的行 j，{data.Points[4].j}\r\n"; data.Num++;
            text += $"{data.Num}，栅格 C 中的点的数量，{C_Points.Count}\r\n"; data.Num++;
            text += $"{data.Num}，栅格 C 中的平均高度，{C_Zaver:f3}\r\n"; data.Num++;
            text += $"{data.Num}，栅格 C 中高度的最大值，{C_Zmax:f3}\r\n"; data.Num++;
            text += $"{data.Num}，栅格 C 中的高度差，{C_Zdiff:f3}\r\n"; data.Num++;
            text += $"{data.Num}，栅格 C 中的高度方差，{C_Sigma2:f3}\r\n"; data.Num++;

            return text;
        }

        public static string Segmentation(DataCenter data)
        {
            string text = null;

            for(int i = 1; i < 3 * 300; i = i +3)
            {
                Plane pl = new Plane();

                //进行平面拟合
                double a = Cal_D(data.Points[i - 1], data.Points[i]);
                double b = Cal_D(data.Points[i], data.Points[i + 1]);
                double c = Cal_D(data.Points[i - 1], data.Points[i + 1]);
                double p = (a + b + c) / 2;
                pl.S = Sqrt(p * (p - a) * (p - b) * (p - c));
                if (pl.S < 0.1) continue;
               
                double x1 = data.Points[i - 1].X; double x2 = data.Points[i].X; double x3 = data.Points[i + 1].X;
                double y1 = data.Points[i - 1].Y; double y2 = data.Points[i].Y; double y3 = data.Points[i + 1].Y;
                double z1 = data.Points[i - 1].Z; double z2 = data.Points[i].Z; double z3 = data.Points[i + 1].Z;
                pl.A = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                pl.B = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                pl.C = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
                pl.D = (-1) * pl.A * x1 - pl.B * y1 - pl.C * z1;

                //内部点、外部点数目计算
                foreach(Point p0 in data.Points)
                {                  
                    double d = Cal_D2(p0, pl);
                    if (d > 0.1)
                    {
                        pl.OutCounts++;
                        pl.OutPoints.Add(p0);
                    }
                    pl.Dis_s.Add(d);
                }
                pl.InCounts = 997 - pl.OutCounts;


                data.Planes_1.Add(pl);
            }

            //寻找最佳分割平面
            Plane plane_best1 = new Plane();
            plane_best1 = data.Planes_1.OrderBy(p => p.InCounts).Last();//最佳分割平面 J1

            for (int i = 1; i < 3 * 80; i = i + 3)
            {
                Plane pl = new Plane();

                //进行平面拟合
                double a = Cal_D(plane_best1.OutPoints[i - 1], plane_best1.OutPoints[i]);
                double b = Cal_D(plane_best1.OutPoints[i], plane_best1.OutPoints[i + 1]);
                double c = Cal_D(plane_best1.OutPoints[i - 1], plane_best1.OutPoints[i + 1]);
                double p = (a + b + c) / 2;
                pl.S = Sqrt(p * (p - a) * (p - b) * (p - c));
                if (pl.S < 0.1) continue;

                double x1 = plane_best1.OutPoints[i - 1].X; double x2 = plane_best1.OutPoints[i].X; double x3 = plane_best1.OutPoints[i + 1].X;
                double y1 = plane_best1.OutPoints[i - 1].Y; double y2 = plane_best1.OutPoints[i].Y; double y3 = plane_best1.OutPoints[i + 1].Y;
                double z1 = plane_best1.OutPoints[i - 1].Z; double z2 = plane_best1.OutPoints[i].Z; double z3 = plane_best1.OutPoints[i + 1].Z;
                pl.A = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                pl.B = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                pl.C = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
                pl.D = (-1) * pl.A * x1 - pl.B * y1 - pl.C * z1;

                //内部点、外部点数目计算
                foreach (Point p0 in plane_best1.OutPoints)
                {
                    double d = Cal_D2(p0, pl);
                    if (d > 0.1)
                    {
                        pl.OutCounts++;//多余，没改
                        pl.OutPoints.Add(p0);
                    }
                    pl.Dis_s.Add(d);
                }
                pl.InCounts =  plane_best1.OutCounts - 3 - pl.OutCounts;

                data.Planes_2.Add(pl);
            }

            Plane plane_best2 = new Plane();
            plane_best2 = data.Planes_2.OrderBy(p => p.InCounts).Last();//最佳分割平面 J2

            //计算点云水平截面投影
            Cal_Projection(data.Points[4], plane_best1);
            Cal_Projection(data.Points[799], plane_best2);

            text += $"{data.Num}，P1-P2-P3 构成三角形的面积，{data.Planes_1[0].S:f6}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的参数 A，{data.Planes_1[0].A:f6}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的参数 B，{data.Planes_1[0].B:f6}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的参数 C，{data.Planes_1[0].C:f6}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的参数 D，{data.Planes_1[0].D:f6}\r\n"; data.Num++;
            text += $"{data.Num}，P1000 到拟合平面 S1 的距离，{Cal_D2(data.Points[999], data.Planes_1[0]):f3}\r\n"; data.Num++;
            text += $"{data.Num}，P5 到拟合平面 S1 的距离，{Cal_D2(data.Points[4], data.Planes_1[0]):f3}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的内部点数量，{data.Planes_1[0].InCounts}\r\n"; data.Num++;
            text += $"{data.Num}，拟合平面 S1 的外部点数量，{data.Planes_1[0].OutCounts}\r\n"; data.Num++;

            text += $"{data.Num}，最佳分割平面 J1 的参数 A，{plane_best1.A:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J1 的参数 B，{plane_best1.B:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J1 的参数 C，{plane_best1.C:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J1 的参数 D，{plane_best1.D:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J1 的内部点数量，{plane_best1.InCounts}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J1 的外部点数量，{plane_best1.OutCounts}\r\n"; data.Num++;

            text += $"{data.Num}，最佳分割平面 J2 的参数 A，{plane_best2.A:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J2 的参数 B，{plane_best2.B:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J2 的参数 C，{plane_best2.C:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J2 的参数 D，{plane_best2.D:f6}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J2 的内部点数量，{plane_best2.InCounts}\r\n"; data.Num++;
            text += $"{data.Num}，最佳分割平面 J2 的外部点数量，{plane_best2.OutCounts}\r\n"; data.Num++;

            text += $"{data.Num}，P5 到最佳分割面（J1）投影坐标 Xt，{data.Points[4].Xt:f3}\r\n"; data.Num++;
            text += $"{data.Num}，P5 到最佳分割面（J1）投影坐标 Yt，{data.Points[4].Yt:f3}\r\n"; data.Num++;
            text += $"{data.Num}，P5 到最佳分割面（J1）投影坐标 Zt，{data.Points[4].Zt:f3}\r\n"; data.Num++;
            text += $"{data.Num}，P800 到最佳分割面（J2）投影坐标 Xt，{data.Points[799].Xt:f3}\r\n"; data.Num++;
            text += $"{data.Num}，P800 到最佳分割面（J2）投影坐标 Yt，{data.Points[799].Yt:f3}\r\n"; data.Num++;
            text += $"{data.Num}，P800 到最佳分割面（J2）投影坐标 Zt，{data.Points[799].Zt:f3}\r\n"; data.Num++;

            return text;
        }

        //计算两点间距离
        public static double Cal_D(Point a, Point b)
        {
            double d = Sqrt(Pow(a.X - b.X, 2) + Pow(a.Y - b.Y, 2) + Pow(a.Z - b.Z, 2));
            return d;
        }

        //计算点到平面距离
        public static double Cal_D2(Point p0, Plane pl)
        {
            double d = Abs(pl.A * p0.X + pl.B * p0.Y + pl.C * p0.Z + pl.D) / Sqrt(pl.A * pl.A + pl.B * pl.B + pl.C * pl.C);
            return d;
        }

        //计算点云水平截面投影
        public static void Cal_Projection(Point p0, Plane pl)
        {
            p0.Xt = ((pl.B * pl.B + pl.C * pl.C) * p0.X - pl.A * (pl.B * p0.Y + pl.C * p0.Z + pl.D)) / (pl.A * pl.A + pl.B * pl.B + pl.C * pl.C);
            p0.Yt = ((pl.A * pl.A + pl.C * pl.C) * p0.Y - pl.B * (pl.A * p0.X + pl.C * p0.Z + pl.D)) / (pl.A * pl.A + pl.B * pl.B + pl.C * pl.C);
            p0.Zt = ((pl.B * pl.B + pl.A * pl.A) * p0.Z - pl.C * (pl.A * p0.X + pl.B * p0.Y + pl.D)) / (pl.A * pl.A + pl.B * pl.B + pl.C * pl.C);
        }
    }
}
