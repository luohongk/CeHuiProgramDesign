using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaDIZhengFan
{
    class DataCenter
    {

        public double a;
        public double e_dao;
        public List<string> start_point = new List<string>();
        public List<string> end_point = new List<string>();

        //数据要素包含起点的经纬度，两个大地方位角，大地线长度
        public List<double> B1 = new List<double>();
        public List<double> L1 = new List<double>();
        public List<double> A12 = new List<double>();
        public List<double> S = new List<double>();
        public List<double> B2 = new List<double>();
        public List<double> L2 = new List<double>();
        public List<double> A21 = new List<double>();
    }
}
