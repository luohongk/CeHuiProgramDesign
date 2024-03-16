using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS_SPP
{
    class DataCenter
    {
        public List<Position> positions = new List<Position>();
        public List<truePosition> truePositions = new List<truePosition>();
    }
    class Position
    {
        public int SatNum;
        public int GPS_Time;

        public string PRN;
        public double x;
        public double y;
        public double z;
        public double SatClock;
        public double Elevation;
        public double L;//伪距
        public double Delay;

    }
    class truePosition
    {
        public Int32 LiYuan;
        public double x;
        public double y;
        public double z;

    }
}
