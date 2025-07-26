using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation1_PointCloudSegmentation
{
    class Plane
    {
        public List<Point> FitPoints = new List<Point>();
        public double A, B, C, D;
        public double S;//三点构成三角形的面积
        public List<double> Dis_s = new List<double>();
        public int OutCounts, InCounts;
        public List<Point> OutPoints = new List<Point>();//外部点集合
    }
}
