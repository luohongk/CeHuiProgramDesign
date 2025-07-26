using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11Curve_Fitting
{
    internal class FileCenter
    {
        public static void ReadFile(string path, DataCenter data)
        {
            StreamReader reader = new StreamReader(path);
            while(!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] buf = line.Split(',');
                Point p = new Point();
                p.ID = Convert.ToInt32(buf[0]);
                p.X = Convert.ToDouble(buf[1]);
                p.Y = Convert.ToDouble(buf[2]);
                data.points.Add(p);
            }
            reader.Close();
        }
        public static void Save_Text(string path, string text)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(text);
            writer.Close();
        }
    }
}
