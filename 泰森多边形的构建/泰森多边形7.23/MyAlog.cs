using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Math;

namespace 泰森多边形7._23
{
    internal class MyAlog
    {
        public List<MyPoint> points=new List<MyPoint>();
        public List<MyPoint> TB=new List<MyPoint>();
        public List<Triangel> triangels = new List<Triangel>();
        public List<Triangel> bad_triangels = new List<Triangel>();
        public List<Edge> edges = new List<Edge>();
        public List<Edge> polyog = new List<Edge>();//多边形边集合
        public List<Triangel> Sum_triangels = new List<Triangel>();//最终三角网
        // 点 P 的泰森多边形的顶点列表（按逆时针排列）
        public Dictionary<MyPoint, List<MyPoint>> VoronoiPolygons = new Dictionary<MyPoint, List<MyPoint>>();
        public string PrintBG()
        {
            SetUpTB();
            SetUpTriangels();
            SetUpTS();
            StringBuilder read=new StringBuilder();
            read.AppendLine($"--------构建凸包点数量--------");
            read.AppendLine($"凸包点数量:{TB.Count} ");
            read.AppendLine($"凸包点面积:{Cal_XD(TB)}");
            read.AppendLine($"--------构建三角{Sum_triangels.Count}");
            Sum_triangels = Sum_triangels.OrderBy(t => Cal_TriangelAra(t)).ToList();
            foreach( var ite in Sum_triangels)
            {
                read.AppendLine($"三角网面积:{Cal_TriangelAra(ite)}");
            }
            read.AppendLine("--------泰森多边形面积（升序排列）--------");

            foreach (var kvp in VoronoiPolygons
             .Select(kv => new { Point = kv.Key, Polygon = kv.Value, Area = Cal_XD(kv.Value) })
             .OrderBy(kv => kv.Area))
            {
                if (TB.Contains(kvp.Point)) continue;
                read.AppendLine($"{kvp.Point.Name} 的泰森多边形面积: {kvp.Area:F3}");
            }


            return read.ToString();
        }
        /// <summary>
        /// 构建凸包点
        /// </summary>
        public void SetUpTB()
        {
            var sort=Cal_sort(points);
            TB.Add(sort[0]);
            TB.Add(sort[1]);
            for(int i=2;i<sort.Count;i++)
            {
                while (TB.Count > 2 && Cal_CJ(TB[TB.Count - 2], TB[TB.Count - 1], sort[i])>0)
                {
                    TB.RemoveAt(TB.Count - 1);
                }
                TB.Add(sort[i]);
            }
           
        }
      
        /// <summary>
        /// 构建三角网
        /// </summary>
        public void SetUpTriangels()
        {
            // 计算所有点的坐标范围
            var x_=points.Average(t=>t.x);
            var y_=points.Average(t=>t.y);
            var add = Math.Pow(10, 10);
            // 构建包含所有点的超级三角形
            MyPoint P1 = new MyPoint("P1", x_,y_+add);
            MyPoint P2 = new MyPoint("P2", x_-add,y_-add);
            MyPoint P3 = new MyPoint("P3", x_+add,y_-add);
            Triangel big=new Triangel("1", P1, P2,P3 );
            triangels.Add(big);
            foreach(var point in points)
            {
                bad_triangels.Clear();
                //第一步
                foreach (var triangel in triangels)
                {
                    var tri_data = Cal_Circle(triangel);
                    double d = Cal_Distance(new MyPoint("", tri_data.X0, tri_data.Y0), point);
                    if(d<tri_data.R)
                    {
                        bad_triangels.Add(triangel);
                    }
                }
                //第二步
                Dictionary<Edge, int> edgeCount = new Dictionary<Edge, int>();

                foreach (var bad in bad_triangels)
                {
                     Edge[] triEdges = {new Edge(bad.p1,bad.p2), new Edge(bad.p2,bad.p3),new Edge(bad.p3,bad.p1)};

                    foreach (var e in triEdges)
                    {
                        if (edgeCount.ContainsKey(e))
                            edgeCount[e]++;
                        else
                            edgeCount[e] = 1;
                    }
                }

                // 保留只出现一次的边（polygon边）
                foreach (var kvp in edgeCount)
                {
                    if (kvp.Value == 1)
                        polyog.Add(kvp.Key);
                }

                edges.Clear();
                var sort=triangels.Except(bad_triangels);//设置一个变量指针传递到tri去
                foreach(var pol in polyog)
                {
                    var tri=new Triangel("",point,pol.p1,pol.p2);
                    triangels.Add(tri);
                }
                polyog.Clear();
                triangels = triangels.Except(bad_triangels).ToList();
            }
            //移除超级三角形
            Sum_triangels = triangels
                .Where(t => t.p1 != P1 && t.p1 != P2 && t.p1 != P3 &&
                         t.p2 != P1 && t.p2 != P2 && t.p2 != P3 &&
                         t.p3 != P1 && t.p3 != P2 && t.p3 != P3)
                         .ToList();
        }
        /// <summary>
        /// 构建泰森多边形
        /// </summary>
        public void SetUpTS()
        {
            // 为每个点构建泰森多边形

            foreach (var point in points)
            {
                if (TB.Contains(point)) continue; // 凸包点跳过，不构造泰森图
                List<Triangel> relatedTriangles = Sum_triangels
                    .Where(t => t.p1 == point || t.p2 == point || t.p3 == point)
                    .ToList();

                List<(double x, double y)> centers = new List<(double x, double y)>();

                foreach (var tri in relatedTriangles)
                {
                    var circle = Cal_Circle(tri);
                    centers.Add((circle.X0, circle.Y0));
                }

                // 排序外接圆圆心形成凸多边形（逆时针）
                var sorted = centers
                    .Select(c => new MyPoint("", c.x, c.y))
                    .OrderBy(p => Cal_Angel(point, p))
                    .ToList();

                VoronoiPolygons[point] = sorted;
            }
        }

        /// <summary>
        /// 判断两条边的公共边
        /// </summary>
        /// <param name="edge1"></param>
        /// <param name="edge2"></param>
        /// <returns></returns>
        public bool Cal_PDEdge(Edge edge1,Edge edge2)
        {
            return edge1.p1 == edge2.p1||edge1.p2 == edge2.p2||
                edge1.p1==edge2.p2||edge1.p2==edge2.p1;
        }
        /// <summary>
        /// 计算叉积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public double Cal_CJ(MyPoint p1,MyPoint p2,MyPoint p3)
        {
            double left = (p1.x - p2.x) * (p3.y - p2.y);
            double right=(p1.y-p2.y)*(p3.x-p2.x);
            return left - right;
        }
        /// <summary>
        /// 逆时针排序点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<MyPoint> Cal_sort(List<MyPoint> points)
        {
            var data = new List<MyPoint>(points);
            var _p = data.OrderBy(t => t.y).ThenBy(t => t.x).First();
            var sort = data.Select(t => new { Point = t, Angel = Cal_Angel(_p, t), Distance = Cal_Distance(_p, t) }).
                OrderBy(t => t.Angel).ThenBy(t => t.Distance).GroupBy(t => t.Angel).Select(t => t.Last()).Select(t => t.Point).ToList();
            return sort;
        }
        /// <summary>
        /// 计算两点的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double Cal_Distance(MyPoint p1,MyPoint p2)
        {
            double x = p1.x - p2.x;
            double y = p1.y - p2.y;
            return Math.Sqrt(x*x + y*y);
        }
        /// <summary>
        /// 计算两点角度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double Cal_Angel(MyPoint p1,MyPoint p2 )
        {
            double x = p2.x-p1.x;
            double y = p2.y-p1.y;
            return Atan2(y,x);
        }
        /// <summary>
        /// 计算三角形面积
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double Cal_TriangelAra(Triangel data)
        {
            double a = Cal_Distance(data.p1, data.p2);
            double b = Cal_Distance(data.p2, data.p3);
            double c = Cal_Distance(data.p3, data.p1);
            double s=(a+b+c)/2;
            return Sqrt(s*(s-a)*(s-b)*(s-c));
        }
        /// <summary>
        /// 鞋带公式计算多边形面积（修正版）
        /// </summary>
        /// <param name="points">按顺序排列的多边形顶点（凸包点）</param>
        /// <returns>多边形面积</returns>
        public double Cal_XD(List<MyPoint> points)
        {
            double sum = 0;
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                MyPoint pCurrent = points[i];
                MyPoint pNext = points[(i + 1) % n]; // 下一个点，最后一个点的下一个是第一个点

                // 累加当前点与下一个点的叉积分量
                sum += pCurrent.x * pNext.y - pNext.x * pCurrent.y;
            }

            // 取绝对值并除以2
            return Math.Abs(sum) / 2.0;
        }
        /// <summary>
        /// 计算外接圆圆心和半径
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public (double R,double X0,double Y0)Cal_Circle(Triangel data)
        {
            double x1 = data.p1.x;
            double y1 = data.p1.y;
            double x2 = data.p2.x;
            double y2 = data.p2.y;
            double x3 = data.p3.x;
            double y3 = data.p3.y;
            double Dx = (x1 * x1 + y1 * y1) * (y2 - y3) + (x2 * x2 + y2 * y2) * (y3 - y1) + (x3 * x3 + y3 * y3) * (y1 - y2);
            double Dy=(x1*x1+y1*y1)*(x3-x2)+(x2*x2+y2*y2)*(x1-x3)+(x3*x3+ y3 * y3)*(x2-x1);
            double A = (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2))*2 ;
            double X0 = Dx / A;
            double Y0 = Dy / A;
            double R = Sqrt(Pow(X0 - x1, 2) + Pow(Y0 - y1, 2));
            return (R, X0, Y0);
        }
    }
}
