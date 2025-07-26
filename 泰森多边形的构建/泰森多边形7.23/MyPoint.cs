using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 泰森多边形7._23
{
    public class MyPoint
    {
        public string Name;
        public double x, y;

        public MyPoint(string name, double x, double y)
        {
            Name = name;
            this.x = x;
            this.y = y;
        }

        public bool Equals(MyPoint other)
        {
            return this.x == other.x && this.y == other.y;
        }
    }

}
