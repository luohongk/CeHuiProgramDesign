using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 泰森多边形7._23
{
    internal class Triangel
    {
        public string id;
        public MyPoint p1;
        public MyPoint p2;
        public MyPoint p3;
        public Triangel()
        {

        }
        public Triangel(string id, MyPoint p1, MyPoint p2, MyPoint p3)
        {
            this.id = id;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
    }
}
