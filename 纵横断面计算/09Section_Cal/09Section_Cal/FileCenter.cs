using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _09Section_Cal
{
    internal class FileCenter
    {
        public static void ReadFile(string path, DataCenter data)
        {
            StreamReader reader = new StreamReader(path);
            string Line = reader.ReadLine();
            string[] buf = Line.Split(',');
            data.H0 = Convert.ToDouble(buf[1]);
            reader.ReadLine(); reader.ReadLine();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] Buf = line.Trim().Split(',');
                Point p = new Point();
                p.Name = Buf[0];
                p.X = Convert.ToDouble(Buf[1]);
                p.Y = Convert.ToDouble(Buf[2]);
                p.H = Convert.ToDouble(Buf[3]);
                if (p.Name[0] == 'K') data.K_Points.Add(p);data.KP_Points.Add(p);
                if (p.Name[0] == 'P') data.P_Points.Add(p);
            }

            //M_Points 也在这里创建
            double m1_x = (data.K_Points[0].X + data.K_Points[1].X) / 2;
            double m1_y = (data.K_Points[0].Y + data.K_Points[1].Y) / 2;
            
            double m2_x = (data.K_Points[2].X + data.K_Points[1].X) / 2;
            double m2_y = (data.K_Points[2].Y + data.K_Points[1].Y) / 2;            

            Point m1 = new Point("M1", m1_x, m1_y);
            Point m2 = new Point("M2", m2_x, m2_y);

            data.M_Points.Add(m1);data.M_Points.Add(m2);

            reader.Close();
        }
        public static void WriteFile(string filename, string text)
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(text);
            writer.Close();
        }
    }
}
