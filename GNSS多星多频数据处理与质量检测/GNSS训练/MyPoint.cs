using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS训练
{
    internal class MyPoint
    {
        //读取数据
        public string Time;
        public string SatName;
        public double P1;
        public double P2;
        public double P3;
        public double phi1;
        public double phi2;
        public double phi3;
        //定义周跳
        public string OneZT = "未作判断";
        public string TwoZT = "未作判断";
        public string ThreeZT = "未作判断";
        //定义是否恢复
        public string OneRecouver="未修复";
        public string TwoRecouver="未修复";
        public string ThreeRecouver = "未修复";
        //定义未恢复
        public double NMV;
        public double phi;
        public double GF1;
        public double GF2;
        public double GF3;
        //修复
        public double phireover1;
        public double phireover21;
        public double phireover22;
        public double phireover3;
        public double residual;
        //多路径误差估算方法
        public double MP1;
        public double MP2;
        public double MP3;
        //相位平滑伪距
        public double Pst;
        public double Ps1;
        public double Ps2;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        public double Ps3;
        public MyPoint()
        {

        }
        public MyPoint(string time, string name, double p1, double p2, double p3, double phi1, double phi2, double phi3)
        {
            Time = time;
            SatName = name;
            P1 = p1;
            P2 = p2;
            P3 = p3;
            this.phi1 = phi1;
            this.phi2 = phi2;
            this.phi3 = phi3;
        }
    }
}
