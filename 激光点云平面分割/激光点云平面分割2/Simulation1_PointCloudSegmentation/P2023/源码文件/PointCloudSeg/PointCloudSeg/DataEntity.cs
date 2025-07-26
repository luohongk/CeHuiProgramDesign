using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudSeg
{
    //数据实体


    public class Pointinfo//点类
    {
        public string Name;
        public double X;
        public double Y;
        public double Z;

        public int labeli;//栅格化位置标签
        public int labelj;

        public int J1;//平面内外部标签

        public string label;//平面标签

        public Pointinfo()//无参构造
        {

        }

        public Pointinfo(string name,double x,double y,double z)//有参构造
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public class ClusterPoint//栅格点类
    {
        public List<Pointinfo> Points = new List<Pointinfo>();//一格中的点
    }
}
