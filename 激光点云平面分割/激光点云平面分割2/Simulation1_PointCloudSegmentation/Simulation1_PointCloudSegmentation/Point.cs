using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation1_PointCloudSegmentation
{
    class Point
    {
        public string PointName;
        public double X, Y, Z;

        public int i, j;//所在栅格编号

        public double Xt, Yt, Zt;//投影坐标
    }
}
