using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11Curve_Fitting
{
    internal class Point
    {
        //点自身的属性
        public int ID;
        public double X;
        public double Y;
        //点 Pi 对应曲线 PiPi+1 属性
        public double Cos_theta, Sin_theta;//x, y 方向梯度
        public double E0, E1, E2, E3;//三次曲线方程 x 项系数
        public double F0, F1, F2, F3;//三次曲线方程 y 项系数        

        public Point(int iD = 0, double x = 0.0, double y = 0.0)
        {
            
            ID = iD;
            X = x;
            Y = y;                      
        }
    }
}
