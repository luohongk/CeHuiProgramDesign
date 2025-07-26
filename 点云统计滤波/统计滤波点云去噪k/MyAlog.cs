using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace 统计滤波点云去噪k
{
    internal class MyAlog
    {
        public List<MyPoint> points=new List<MyPoint>();
        public List<MyPoint> [,,]SG = new List<MyPoint>[10,10,10];//三维的网格
        public int K = 10;//这里定义k为10
        public List<MyPoint>ZSPoints= new List<MyPoint>();
        
        public string PrintBG()
        {
            initSG();
            Add_SG();
            StringBuilder read = new StringBuilder();
            read.AppendLine($"---------------------输出看K临近点是否正确------------------------");
            read.AppendLine($"点云格（1,2，0）点一共有:{SG[1,2,0].Count}");
            read.AppendLine($"找出点云格中的点并标记出它的最近点测试10:{points[10].id}");
            var st = Cal_KPoint(points[10]);
            foreach(var ite in st)
            {
                read.AppendLine($"他的最近10个点:{ite.id}");
            }
            bjpoint = points[10];//用来绘图的标记点
            ZJpoint = st;
            read.AppendLine($"{ZJpoint.Count} points");
            read.AppendLine($"---------------------输出标准差------------------------");
            foreach (var ite in SG[1,2,0])
            {
                read.AppendLine($"{ite.id}在测试区（1,2,0)点的标准差:{ite.BZC}");
            }
            read.AppendLine($"噪声点个数在1,2,0）区域:{ZSPoints.Count}");
            return read.ToString();
        }
        public void initSG()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    for(int k = 0; k < 10; k++)
                    {
                        SG[i, j, k] = new List<MyPoint>();
                    }
                    
                }
            }
        }
        /// <summary>
        /// 计算三维距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public double Cal_ThreeDistance(MyPoint p1,MyPoint p2)
        {
            double x1 = p1.x - p2.x;
            double y1 = p1.y - p2.y ;
            double z1=p1.z- p2.z;
            return Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1);
        }
        /// <summary>
        /// 计算K近邻点
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        public List<MyPoint>Cal_KPoint(MyPoint p1)
        {
            var data = new List<MyPoint>(points);//防止破坏点集
            var SumPoints=new List<MyPoint>();//存储这个点的集合;
            foreach(var ite in data)
            {
                if(ite.id==p1.id)//跳过自己
                {
                    continue;
                }
                ite.d= Cal_TwoDistance(p1,ite);
                SumPoints.Add(ite);
            }
            SumPoints=SumPoints.OrderBy(t=>t.d).ToList();
            MyPoint pk = SumPoints[K-1];//这个标记出第K个点
            return SumPoints.Where(t=>t.d<=pk.d).ToList();//这样筛选因为已经排过顺序了
        }
        public double Cal_TwoDistance(MyPoint p1, MyPoint p2)
        {
            double x1 = p1.x - p2.x;
            double y1 = p1.y - p2.y;
            double z1 = p1.z - p2.z;
            return Math.Sqrt(x1 * x1 + y1 * y1 );
        }

        /// <summary>
        /// 判断均值,x,y,z方向的
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public (double XAverage,double YAverage,double ZAverage)Cal_Pd(List<MyPoint> points)
        {
            double x = (double)(points.Sum(t => t.x) / K);
            double y = (double)(points.Sum(t => t.y) / K);
            double z = (double)(points.Sum(t => t.z) / K);
            return(x,y,z);
        }
        /// <summary>
        /// 计算标准差
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public double Cal_BZC(List<MyPoint> points)
        {
            var XYZ_Average = Cal_Pd(points);//计算出了每个的均值
            double sum_x = points.Sum(t => Math.Pow(t.x - XYZ_Average.XAverage, 2));//计算x方向的
            double sum_y = points.Sum(t => Math.Pow(t.y - XYZ_Average.YAverage, 2));
            double sum_z = points.Sum(t => Math.Pow(t.z - XYZ_Average.ZAverage, 2));
            return Math.Sqrt((sum_x+sum_y+sum_z)/K);

        }
        /// <summary>
        /// 判断点是否是噪声点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="BZC"></param>
        /// <returns></returns>
        public bool Cal_T(MyPoint p1,double BZC)
        {
            double T = 1.0 * BZC;
            bool pd = true;//判断该点是否是噪声点
            if(p1.d<T)
            {
                pd = false;
            }
            return pd;
        }
        //判断最近点
        public List<MyPoint> ZJpoint = new List<MyPoint>();
        public MyPoint bjpoint=new MyPoint();
        public void Add_SG()
        {
            //先添加到点云里面
            foreach(var ite in points)
            {
                int x = (int)Math.Floor(ite.x / 10.0);
                int y = (int)Math.Floor(ite.y / 10.0);
                SG[x,y,0].Add(ite);
            }
            //这里网格里面的点做统计
            foreach (var ite in SG[1,2,0])  // 对网格中的每个点ite进行判断
            {
                var neighbors = Cal_KPoint(ite);  // 找ite的近邻点
                if (neighbors.Count == 0) continue;  // 无近邻点跳过
                var (xAvg, yAvg, zAvg) = Cal_Pd(neighbors);  // 近邻点的均值
                ite.BZC = Cal_BZC(neighbors);  // 近邻点的标准差
                // 计算当前点ite到均值的距离
                double distanceToMean = Cal_ThreeDistance(ite, new MyPoint(xAvg, yAvg, zAvg));
                // 判断ite是否为噪声点
                if (Cal_T(ite,ite.BZC)) 
                {
                    ZSPoints.Add(ite);
                }
            }
        }
    }
}
