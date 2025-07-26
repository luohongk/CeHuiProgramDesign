using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudSeg
{
    public class Cal//栅格相关计算类
    {
        /// <summary>
        /// 计算平均高度
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetAveH(List<Pointinfo> points)
        {
            double sum = 0;
            for(int i=0;i< points.Count; i++)
            {
                sum += points[i].Z;
            }

            return sum / points.Count;
        }

        /// <summary>
        /// 找最大高度
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetZmax(List<Pointinfo> points)
        {
            double zmax = -1000000;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Z > zmax) zmax = points[i].Z;
            }

            return zmax;
        }

        /// <summary>
        /// 计算高度差
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetDiffH(List<Pointinfo> points)
        {
            double d = 0,zmin=100000;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Z < zmin) zmin = points[i].Z;
            }
            double zmax = GetZmax(points);
            d = zmax - zmin;
            return d;
        }

        /// <summary>
        /// 计算高度方差
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetS2(List<Pointinfo> points)
        {
            double S = 0;
            double ave = GetAveH(points);
            for (int i = 0; i < points.Count; i++)
            {
                S += Math.Pow(points[i].Z-ave,2);
            }

            return S / points.Count;
        }
    }
}
