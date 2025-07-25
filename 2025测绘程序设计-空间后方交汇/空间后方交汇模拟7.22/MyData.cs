using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空间后方交汇模拟7._22
{
    internal class MyData
    {
        public string id;
        public double X;
        public double Y;
        public double Z;
        public double x;
        public double y;
        //求近似值
        public double dX;
        public double dY;
        public double dZ;
        //近视点坐标
        public double xap;
        public double yap;

        public MyData()
        {

        }
        public MyData(string id, double x, double y, double z, double X, double Y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.X = X;
            this.Y = Y;
            this.Z = z;
        }
    }
}
