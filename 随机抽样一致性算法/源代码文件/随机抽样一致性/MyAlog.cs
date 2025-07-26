using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空间_后方交汇
{
    class MyAlog
    {
        public static List<MyPoint> points = new List<MyPoint>();//原始数据集
        public int PointNumber;//点云总数量
        public List<MyPoint>[,] SGPoints = new List<MyPoint>[10, 10];//栅格
        public int P5i;
        public int P5j;
        public double Chigh;//平均高度
        public double ChighC;//高度差
        public double ChighFC;
        public List<MyPoint> S1number = new List<MyPoint>();
        public double MAXpoint = 0;//J1的内部点数量（没-3)
        public  (double A, double B, double C, double D) MAXPM = (0, 0, 0, 0);//J1的平面系数
        public double J2point = 0;//J2的内部点数量
        public (double A, double B, double C, double D) J2PM = (0, 0, 0, 0);//J2的平面系数
        public List<MyPoint> j2points = new List<MyPoint>();//J2剔除点J1的集合
        public List<MyPoint> maxPoint1 = new List<MyPoint>();//J1点的集合
        public List<MyPoint> maxPoint2 = new List<MyPoint>();//J2点的集合
        public List<MyPoint> J1points = new List<MyPoint>();//内点集合
        /// <summary>
        /// 输出报告
        /// </summary>
        /// <returns></returns>
        public string PringBG()
        {
            initSG();
            SGFG();
            RANSAC();
            double minx = points.OrderBy(t => t.x).First().x;
            double maxx = points.OrderBy(t => t.x).Last().x;
            double miny = points.OrderBy(t => t.y).First().y;
            double maxy = points.OrderBy(t => t.y).Last().y;
            double minz = points.OrderBy(t => t.z).First().z;
            double maxz = points.OrderBy(t => t.z).Last().z;
            var S1 = CalPMXS(points[0], points[1], points[2]);
            for (int j = 0; j < points.Count; j++)
            {
                double d = PMdistance(points[j], S1.A, S1.B, S1.C, S1.D);
                if (d < 0.1)
                {
                    S1number.Add(points[j]);
                }
            }
            var P5J1 = CalTY(MAXPM.A, MAXPM.B, MAXPM.C, MAXPM.D, points[4]);//计算投影系数
            var P800J1 = CalTY(MAXPM.A, MAXPM.B, MAXPM.C, MAXPM.D, points[799]);
            StringBuilder read = new StringBuilder();
            read.AppendLine($"P5的坐标分量x:{points[4].x}");
            read.AppendLine($"P5的坐标分量y:{points[4].y}");
            read.AppendLine($"P5的坐标分量z:{points[4].z}");
            read.AppendLine($"坐标分量x的最小值:{minx}");
            read.AppendLine($"坐标分量x的最大值:{maxx}");
            read.AppendLine($"坐标分量y的最小值:{miny}");
            read.AppendLine($"坐标分量y的最大值:{maxy}");
            read.AppendLine($"坐标分量z的最小值:{minz}");
            read.AppendLine($"坐标分量z的最大值:{maxz}");
            read.AppendLine($"P5点的所在栅格的行i:{P5i}");
            read.AppendLine($"P5点的所在栅格的行j:{P5j}");
            read.AppendLine($"栅格C中的点数量:{SGPoints[2, 3].Count}");
            read.AppendLine($"栅格C中的平均高度:{Chigh:F3}");
            read.AppendLine($"栅格C中的高度最大值:{SGPoints[2, 3].OrderBy(t => t.z).Last().z:F3}");
            read.AppendLine($"栅格C中的高度差:{ChighC:F3}");
            read.AppendLine($"栅格C中的高度方差:{ChighFC:F3}");
            read.AppendLine($"P1-P2-P3构成的三角形面积:{CalTriangel(points[0], points[1], points[2]):F6}");
            read.AppendLine($"拟合平面S1的参数A:{S1.A:F6}");
            read.AppendLine($"拟合平面S1的参数B:{S1.B:F6}");
            read.AppendLine($"拟合平面S1的参数C:{S1.C:F6}");
            read.AppendLine($"拟合平面S1的参数D:{S1.D:F6}");
            read.AppendLine($"P1000到拟合平面S1的距离:{PMdistance(points[999], S1.A, S1.B, S1.C, S1.D):f3}");
            read.AppendLine($"P5到拟合平面S1的距离:{PMdistance(points[4], S1.A, S1.B, S1.C, S1.D):f3}");
            read.AppendLine($"拟合平面S1的内部点数量:{S1number.Count-3}");//要减去他们本身
            read.AppendLine($"拟合平面S1的外部点数量:{points.Count - S1number.Count}");
            read.AppendLine($"最佳分割平面J1的参数A:{MAXPM.A}");
            read.AppendLine($"最佳分割平面J1的参数B:{MAXPM.B}");
            read.AppendLine($"最佳分割平面J1的参数C:{MAXPM.C}");
            read.AppendLine($"最佳分割平面J1的参数D:{MAXPM.D}");
            read.AppendLine($"最佳分割平面J1的内部点数量:{MAXpoint-3}");
            read.AppendLine($"最近分割平面J1的外部点数量:{points.Count - MAXpoint}");
            read.AppendLine($"最近分割平面J2的参数A:{J2PM.A}");
            read.AppendLine($"最近分割平面J2的参数B:{J2PM.B}");
            read.AppendLine($"最近分割平面J2的参数C:{J2PM.C}");
            read.AppendLine($"最近分割平面J2的参数D:{J2PM.D}");
            read.AppendLine($"最佳分割平面J2的内部点数量:{J2point-3}");
            read.AppendLine($"最近分割平面J2的外部点数量:{j2points.Count-J2point}");
            read.AppendLine($"P5到最佳分割平面(J1)的投影坐标xi:{P5J1.x:f3}");
            read.AppendLine($"P5到最佳分割平面(J1)的投影坐标yi:{P5J1.y:f3}");
            read.AppendLine($"P5到最佳分割平面(J1)的投影坐标zi:{P5J1.z:f2}");
            read.AppendLine($"P800到最佳分割平面(J1)的投影坐标xi:{P800J1.x:f3}");
            read.AppendLine($"P800到最佳分割平面(J1)的投影坐标yi:{P800J1.y:f3}");
            read.AppendLine($"P800到最佳分割平面(J1)的投影坐标zi:{P800J1.z:f3}");

            return read.ToString();
        }
        /// <summary>
        /// 栅格初始化
        /// </summary>

        public void initSG()
        {
            SGPoints = new List<MyPoint>[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SGPoints[i, j] = new List<MyPoint>();
                }
            }
        }
        /// <summary>
        /// 栅格分割
        /// </summary>
        public void SGFG()
        {
            int dx = 10;
            int dy = 10;
            for (int i = 0; i < points.Count; i++)
            {
                int j_sg = (int)Math.Floor((points[i].y / dy));
                int i_sg = (int)Math.Floor((points[i].x / dx));
                SGPoints[i_sg, j_sg].Add(points[i]);
            }
            double zsum = SGPoints[2, 3].Sum(t => t.z);
            Chigh = zsum / SGPoints[2, 3].Count;
            double zmin = SGPoints[2, 3].OrderBy(t => t.z).First().z;
            double zmax = SGPoints[2, 3].OrderBy(t => t.z).Last().z;
            ChighC = zmax - zmin;
            ChighFC = SGPoints[2, 3].Sum(t => Math.Pow(t.z - Chigh, 2)) / SGPoints[2, 3].Count;
        }
        /// <summary>
        /// 进行随机抽样一致性算法
        /// </summary>
        public void RANSAC()
        {
            double FZ = 0.1; // 阀值
            int maxnumber = 0;
            List<MyPoint> bestPointList = new List<MyPoint>(); // 保存最佳内点列表
            //计算J1的
            for (int i = 0; i < 300; i++)
            {
                maxPoint1.Clear();//确保每次进行一次都清除
                int p1 = i * 3 ;
                int p2 = (1 + i * 3) ;
                int p3 = (2 + i * 3) ;

                var PM = CalPMXS(points[p1], points[p2], points[p3]);
                maxnumber = i;

                for (int j = 0; j < points.Count; j++)
                {
                    double d = PMdistance(points[j], PM.A, PM.B, PM.C, PM.D);
                    if (d < FZ)
                    {
                        maxPoint1.Add(points[j]);
                    }
                }

                if (maxPoint1.Count > MAXpoint)//进行条件判断迭代
                {
                    MAXpoint = maxPoint1.Count;
                    MAXPM = PM;
                    bestPointList.Clear();
                    bestPointList.AddRange(maxPoint1);
                    J1points = bestPointList;
                }
            }
            //计算J2的
            j2points = points.Except(J1points).ToList();//剔除J1点
            for (int i = 0; i < 80; i++)
            {
                maxPoint2.Clear();
                // 确保索引不越界
                int p1 = i * 3;
                int p2 = (1 + i * 3);
                int p3 = (2 + i * 3);

                var PM = CalPMXS(j2points[p1], j2points[p2], j2points[p3]);
                for (int j = 0; j < j2points.Count; j++)
                {
                    double d = PMdistance(j2points[j], PM.A, PM.B, PM.C, PM.D);
                    if (d < FZ)
                    {
                        maxPoint2.Add(j2points[j]);
                    }
                }

                if (maxPoint2.Count > J2point)
                {
                    J2point = maxPoint2.Count;
                    J2PM = PM;
                    
                }
            }

        }
        /// <summary>
        /// 计算两点间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double distance(MyPoint p1,MyPoint p2)
        {
            double x1 = p1.x - p2.x;
            double y1 = p1.y - p2.y;
            double z1 = p1.z - p2.z;
            return Math.Sqrt(x1 * x1 + y1 * y1+z1*z1);
        }
        /// <summary>
        /// 计算三角形面积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public double CalTriangel(MyPoint p1,MyPoint p2,MyPoint p3)
        {
            double a = distance(p1, p2);
            double b = distance(p2, p3);
            double c = distance(p3, p1);
            double p = (a + b + c) / 2;
            double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return S;
        }
        /// <summary>
        /// 计算平面系数
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public (double A,double B,double C,double D)CalPMXS(MyPoint p1,MyPoint p2,MyPoint p3)
        {
            double a = (p2.y - p1.y) * (p3.z - p1.z) - ((p3.y - p1.y) * (p2.z - p1.z));
            double b = (p2.z - p1.z) * (p3.x - p1.x) - ((p3.z - p1.z) * (p2.x - p1.x));
            double c = (p2.x - p1.x) * (p3.y - p1.y) - ((p3.x - p1.x) * (p2.y - p1.y));
            double D = (-1 * a * p1.x) - (b * p1.y) - (c * p1.z);
            return (a, b, c, D);
        }
        /// <summary>
        /// 计算点到平面的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public double PMdistance(MyPoint p1,double A,double B,double C,double D)
        {
            double over = Math.Abs(A * p1.x + B * p1.y + C * p1.z + D);
            double under = Math.Sqrt(A * A + B * B + C * C);
            return over / under;
        }
        /// <summary>
        /// 计算投影系数
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public (double x ,double y,double z)CalTY(double A,double B,double C,double D,MyPoint p1)
        {
            double xt = ((B * B + C * C) * p1.x - A * (B * p1.y + C * p1.z + D)) / (A * A + C * C + B * B);
            double yt = ((A * A + C * C) * p1.y - B*(A * p1.x + C * p1.z + D)) / (A * A + C * C + B * B);
            double zt = ((A * A + B * B) * p1.y - C * (A * p1.x + B * p1.y+D)) / (A * A + C * C + B * B);
            return (xt, yt, zt);
        }
    }

    
}
