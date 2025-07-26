using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09Section_Cal
{
    internal class DataCenter
    {
        public double H0;
        public List<Point> KP_Points;//K 和 P 的集合
        public List<Point> K_Points;//关键点
        public List<Point> P_Points;//坐标已知的离散点
        public List<Point> M_Points;//两个横断面中间点

        public DataCenter() 
        { 
            H0 = 0;
            K_Points = new List<Point>();
            P_Points = new List<Point>();
            KP_Points = new List<Point>();
            M_Points = new List<Point>();
        }
    }
}
