using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace _10Trop_NEIL
{
    internal class Epoch
    {
        public string Station_Name;

        public string YMD;//公历时间 YYYYMMDD
        public int DOY;//年积日

        public double L;//经度
        public double B;//纬度

        public double G_H;//大地高
        public double O_H;//正高

        public int E;//高度角
        public double E_rad;//弧度制高度角

        public double mwE;//湿分量投影函数值
        public double aw_avg, bw_avg, cw_avg;//湿分量投影函数系数  注：= aw, bw, cw

        public double mdE;//干分量投影函数值
        public double ad_avg, bd_avg, cd_avg;//干分量投影函数系数 1
        public double ad_amp, bd_amp, cd_amp;//干分量投影函数系数 2  
        public double ad, bd, cd;//干分量投影函数系数 = 1 + 2

        public double ZHD;
        public double ZWD;
        public double delta_S;//延迟改正数

        //构造函数
        public Epoch(string sname = "unkown", string ymd = "unknown", int doy = 0, 
            double l = 0.0, double b = 0.0, double gh = 0.0, double oh = 0.0, int e = 0, double erad = 0.0 ) 
        { 
            Station_Name = sname;
            YMD = ymd;
            DOY = doy;
            L = l;
            B = b;
            G_H = gh; 
            O_H = oh; 
            E = e;
            E_rad = erad;
        }
        //公历日转年积日
        public int To_DOY(string ymd)
        {
            int doy = 0;
            int year = Convert.ToInt32(YMD.Substring(0, 4));
            int month = Convert.ToInt32(YMD.Substring(4, 2));
            int day = Convert.ToInt32(YMD.Substring(6, 2));

            List<int> days_in_month = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };//存储平年 1-12 月的天数
            if(year % 4 == 0 || year % 400 == 0)
            {
                days_in_month[1] = 29;//闰年 2 月 29 天
            }

            doy = days_in_month.Take(month - 1).Sum() + day;//LINQ 语句

            return doy;
        }
        //大地高转正高
        public double To_O_H(double gh)
        {
            double oh = 0.0;

            oh = gh;//不太知道怎么转换,直接用大地高结果对得上

            return oh;
        }
    }
}
