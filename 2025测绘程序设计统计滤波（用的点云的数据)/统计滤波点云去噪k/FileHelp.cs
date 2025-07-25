using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 统计滤波点云去噪k
{
    internal class FileHelp
    {
        public static void initable(DataTable table)
        {
            table.Clear();
            table.Columns.Add("id",typeof(string));
            table.Columns.Add("x",typeof(double));
            table.Columns.Add("y",typeof(double));
            table.Columns.Add("z",typeof(double));
        }
        public static void Open(MyAlog data,DataTable table)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "读取文件";
            open.Filter = "文本文件|*.txt";
            if(open.ShowDialog() == DialogResult.OK)
            {
                using(var sr=new StreamReader(open.FileName))
                {
                    sr.ReadLine();
                    sr.ReadLine();
                    while(!sr.EndOfStream)
                    {
                        var bvr = sr.ReadLine().Split(',');
                        MyPoint point=new MyPoint(bvr[0], double.Parse(bvr[1]), double.Parse(bvr[2]),double.Parse(bvr[3]));
                        data.points.Add(point);
                    }
                }
            }
            foreach(var ite in data.points)
            {
                DataRow rows = table.NewRow();
                rows["id"] = ite.id;
                rows["x"] = ite.x;
                rows["y"] = ite.y;
                rows["z"] = ite.z;
                table.Rows.Add(rows);
            }
        }
        public static void Save(string data)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存结果";
            save.Filter = "文本文件|*.txt";
            if(save.ShowDialog() == DialogResult.OK)
            {
                using (var sr = new StreamWriter(save.FileName))
                {
                    sr.Write(data);
                }
            }
        }
        public static void Help()
        {
            MessageBox.Show("欢迎使用本程序!\n1.先点击打开\n2.再点击计算\n3.最后点击保存");
        }
        public static void Draw(Chart chart,MyAlog data)
        {
            Series Point = new Series("原始点");
            Point.ChartType = SeriesChartType.Point;
            Point.Color = System.Drawing.Color.Blue;
            Point.MarkerStyle = MarkerStyle.Circle;
            foreach(var ite in data.points)
            {
                Point.Points.AddXY(ite.x,ite.y);
            }
            chart.Series.Add(Point);
            Series ZSPoint = new Series("噪声点");
            ZSPoint.ChartType = SeriesChartType.Point;
            ZSPoint.Color = System.Drawing.Color.Red;
            ZSPoint.MarkerStyle = MarkerStyle.Circle;
            foreach (var ite in data.ZSPoints)
            {
                ZSPoint.Points.AddXY(ite.x, ite.y);
            }
            chart.Series.Add(ZSPoint);
            Series GLPOINT = new Series("举例点");
            GLPOINT.ChartType = SeriesChartType.Point;
            GLPOINT.Color = System.Drawing.Color.Black;
            GLPOINT.MarkerStyle = MarkerStyle.Circle;
            GLPOINT.MarkerSize = 10;
            GLPOINT.Points.AddXY(data.bjpoint.x, data.bjpoint.y);
            chart.Series.Add(GLPOINT);
            Series KPoint = new Series("最近Kpoint");
            KPoint.ChartType = SeriesChartType.Point;
            KPoint.Color = System.Drawing.Color.Brown;
            KPoint.MarkerStyle = MarkerStyle.Circle;
            foreach(var ite in data.ZJpoint)
            {
                KPoint.Points.AddXY(ite.x,ite.y);
            }
            chart.Series.Add(KPoint);

        }
    }
}
