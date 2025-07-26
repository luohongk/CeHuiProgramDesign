using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07GeodeticLine
{
    internal class DataCenter
    {
        public double B1, L1, B2, L2;
        public double b,e2,e12;//

        //构造函数

        //度分秒 ddmmss 转换为弧度 rad
        public double ddmmssTorad(double dms)
        {
            double rad,deg,min,sec;

            deg = (int)(dms);
            min = (int)((dms - deg) * 100);
            sec = dms * 10000 - deg * 10000 - min * 100;

            rad = (deg + min / 60.0 + sec / 3600.0) / 180.0 * Math.PI;

            return rad;
        }
    }
}
