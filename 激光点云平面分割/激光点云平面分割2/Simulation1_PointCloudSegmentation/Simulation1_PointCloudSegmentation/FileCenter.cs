using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Simulation1_PointCloudSegmentation
{
    class FileCenter
    {
        public static string ReadFile(string path, DataCenter data, DataGridView view)
        {
            string text = null;

            StreamReader reader = new StreamReader(path);
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                string[] buf = reader.ReadLine().Split(',');
                Point p = new Point();
                p.PointName = buf[0];
                p.X = Convert.ToDouble(buf[1]);
                p.Y = Convert.ToDouble(buf[2]);
                p.Z = Convert.ToDouble(buf[3]);
                data.Points.Add(p);
            }
            reader.Close();

            //表格显示原始数据
            foreach(Point p in data.Points)
            {
                DataGridViewRow row = new DataGridViewRow();
                string[] values = { p.PointName, p.X.ToString(), p.Y.ToString(), p.Z.ToString() };
                foreach(string value in values)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = value;
                    row.Cells.Add(cell);
                }
                view.Rows.Add(row);
            }

            text += $"{data.Num}，P5 的坐标分量 x，{data.Points[4].X}\r\n"; data.Num++;
            text += $"{data.Num}，P5 的坐标分量 y，{data.Points[4].Y}\r\n"; data.Num++;
            text += $"{data.Num}，P5 的坐标分量 z，{data.Points[4].Z}\r\n"; data.Num++;

            double Xmin = data.Points.OrderBy(p => p.X).First().X;
            double Xmax = data.Points.OrderBy(p => p.X).Last().X;
            double Ymin = data.Points.OrderBy(p => p.Y).First().Y;
            double Ymax = data.Points.OrderBy(p => p.Y).Last().Y;
            double Zmin = data.Points.OrderBy(p => p.Z).First().Z;
            double Zmax = data.Points.OrderBy(p => p.Z).Last().Z;

            text += $"{data.Num}，坐标分量 x 的最小值 Xmin，{Xmin}\r\n"; data.Num++;
            text += $"{data.Num}，坐标分量 x 的最大值 Xmax，{Xmax}\r\n"; data.Num++;
            text += $"{data.Num}，坐标分量 y 的最小值 Ymin，{Ymin}\r\n"; data.Num++;
            text += $"{data.Num}，坐标分量 y 的最大值 Ymax，{Ymax}\r\n"; data.Num++;
            text += $"{data.Num}，坐标分量 z 的最小值 Zmin，{Zmin}\r\n"; data.Num++;
            text += $"{data.Num}，坐标分量 z 的最大值 Zmax，{Zmax}\r\n"; data.Num++;

            return text;
        }
    }
}
