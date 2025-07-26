using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 统计滤波点云去噪k
{
    internal class MyPoint
    {
        public string id;
        public double x;
        public double y;
        public double z;
        public double d;//计算每个点之间的距离
        public double BZC;
        public MyPoint(string id, double x, double y, double z)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public MyPoint()
        {

        }
        public MyPoint(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z; 
        }

    }
}
