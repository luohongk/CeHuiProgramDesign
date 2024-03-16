using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DianYunFenGe
{
    class DataCenter
    {
        public double allPointNum;
        public List<Point> points = new List<Point>();

        public List<double> points_X = new List<double>();
        public List<double> points_Y = new List<double>();
        public List<double> points_Z = new List<double>();

    }

    class Point
    {
        public string pointName;
        public double X;
        public double Y;
        public double Z;
        public string biaoshi;
        public double position_i;
        public double position_j;
    }
}
