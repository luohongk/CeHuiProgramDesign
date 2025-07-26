using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _09Section_Cal
{
    internal class Point
    {
        public string Name;
        public double X;
        public double Y;
        public double H;
        public List<Point> Closest_Points;

        //构造函数
        public Point(string name = "unknown", double x = 0.0, double y = 0.0, double h = 0.0)
        {
            Name = name;
            X = x;
            Y = y;
            H = h;
            Closest_Points = new List<Point>();
        }
        
    }
}
