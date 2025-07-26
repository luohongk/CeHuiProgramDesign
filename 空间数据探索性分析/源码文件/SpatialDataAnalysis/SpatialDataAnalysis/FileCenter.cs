using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SpatialDataAnalysis
{
    class FileCenter
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static string ReadFile(string path, DataCenter data, DataGridView view)
        {
            string text = null;
            StreamReader reader = new StreamReader(path);
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                string[] buf = reader.ReadLine().Split(',');
                Point p = new Point();
                p.ID = buf[0];
                p.X = Convert.ToDouble(buf[1]);
                p.Y = Convert.ToDouble(buf[2]);
                p.AreaCode = Convert.ToInt32(buf[3]);
                data.Points.Add(p);
            }
            reader.Close();

            foreach (Point p in data.Points)
            {
                DataGridViewRow row = new DataGridViewRow();
                string[] values = { p.ID, p.X.ToString(), p.Y.ToString(), p.AreaCode.ToString() };
                foreach (string value in values)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = value;
                    row.Cells.Add(cell);
                }
                view.Rows.Add(row);
            }

            text += "序号,说明,计算结果\r\n";
            text += $"{data.Number}，P6 的坐标 x，{data.Points[5].X:f3}\r\n"; data.Number++;
            text += $"{data.Number}，P6 的坐标 y，{data.Points[5].Y:f3}\r\n"; data.Number++;
            text += $"{data.Number}，P6 的区号，{data.Points[5].AreaCode}\r\n"; data.Number++;
            return text;
        }
    }
}
