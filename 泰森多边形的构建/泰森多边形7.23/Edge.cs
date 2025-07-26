using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 泰森多边形7._23
{
    public class Edge
    {
        public MyPoint p1 { get; set; }
        public MyPoint p2 { get; set; }

        public Edge(MyPoint p1, MyPoint p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        // 判断两条边是否为同一条（正向或反向）
        public override bool Equals(object obj)
        {
            // 检查是否为同一实例
            if (ReferenceEquals(this, obj))
                return true;

            // 检查是否为Edge类型
            if (!(obj is Edge edge))
                return false;

            // 比较两个端点（考虑正向和反向）
            return (p1.Equals(edge.p1) && p2.Equals(edge.p2)) ||
                   (p1.Equals(edge.p2) && p2.Equals(edge.p1));
        }

        // 兼容旧版本的GetHashCode实现
        public override int GetHashCode()
        {
            return 0;
        }
    }
}
