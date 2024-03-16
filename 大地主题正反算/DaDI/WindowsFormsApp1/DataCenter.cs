using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class DataCenter
    {

       

         #region 数据定义
                //椭球参数
        public  double a, b, e1,e2, f;
        public List<double> B1 = new List<double>();
        public List<double> L1 = new List<double>();
        public List<double> S = new List<double>();
        public List<double> A1 = new List<double>();
        public List<double> B2 = new List<double>();
        public List<double> L2 = new List<double>();
        public List<double> A2 = new List<double>();
        #endregion

        #region 数据的初始化
        public static void initial(DataCenter dc)
        {
            dc.a = dc.b / (1 - 1/dc.f);
            dc.e1 = Math.Sqrt((Math.Pow(dc.a, 2) - Math.Pow(dc.b, 2)) / Math.Pow(dc.a, 2));
            dc.e2 = Math.Sqrt((Math.Pow(dc.a, 2) - Math.Pow(dc.b, 2)) / Math.Pow(dc.b, 2));
            dc.B1.Clear();
            dc.L1.Clear();
            dc.S.Clear();
            dc.A1.Clear();
            dc.B2.Clear();
            dc.L2.Clear();
            dc.A2.Clear();
        }
        #endregion 数据转换

        #region 度分秒转弧度
        public static double dmstohd(double dms)
        {
            int i;
            if(dms<0)
            {
                i = -1;
                dms = Math.Abs(dms);
            }
            else
            {
                i = 1;
            }

            double d, m, s,tem1, hud,dms_all;
            d = Math.Floor(dms);
            tem1 = (dms-d) * 100;
            m = Math.Floor(tem1);
            s = ((dms - d) * 100 - m) * 100;
            dms_all = d + m / 60 + s / 3600;
            hud = i *dms_all / 180 * Math.PI;
            return hud;
        }
        #endregion


        #region 弧度转度分秒
        public static double hutudms(double hudu)
        {
            int i=1;
            if(hudu<0)
            {
                i = -1;
                hudu = Math.Abs(hudu);
            }
            double dms_all,d,m,s,dms;
            dms_all = hudu / Math.PI * 180;
            d = Math.Floor(dms_all);
            m = Math.Floor((dms_all - d) * 60);
            s = ((dms_all - d) * 60 - m) * 60;
            dms =i*Math.Round(i* (d + m / 100 + s / 10000),10);
            return dms;
        }
       
        #endregion
        }
}
