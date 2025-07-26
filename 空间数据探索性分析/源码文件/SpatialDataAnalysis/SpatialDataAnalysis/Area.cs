using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialDataAnalysis
{
    class Area
    {
        public int Code;
        public int PointsNum;//事件数
        public List<Point> AreaPoints = new List<Point>();
        public double AxAver, AyAver;
        public double I;//局部莫兰指数
        public double Z;//Z 得分  
    }
}
