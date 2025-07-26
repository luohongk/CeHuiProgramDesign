using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _10Trop_NEIL
{
    internal class FileCenter
    {
        public static void Read_File(string path, DataCenter data)
        {
            StreamReader reader = new StreamReader(path);
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                string Line = reader.ReadLine();
                string[] buf = Line.Split(',');

                Epoch e = new Epoch();

                e.Station_Name = buf[0];
                e.YMD = buf[1];
                e.DOY = e.To_DOY(buf[1]);                
                e.L = Convert.ToDouble(buf[2]);
                e.B = Convert.ToDouble(buf[3]);
                e.G_H = Convert.ToDouble(buf[4]);
                e.O_H = e.To_O_H(Convert.ToDouble(buf[4]));
                e.ZHD = 2.29951 * Math.Pow(Math.E, -0.000116 * e.O_H);
                e.ZWD = 0.1;
                e.E = Convert.ToInt32(buf[5]);
                e.E_rad = Convert.ToDouble(buf[5]) / 180 * Math.PI;

                data.epoches.Add(e);
            }

        }
    }
}
