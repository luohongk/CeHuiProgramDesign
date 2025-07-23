using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 泰森多边形7._23
{
    internal class FileHelp
    {
        public static void initable(DataTable table)
        {
            table.Clear();
            table.Columns.Add("id",typeof(string));
            table.Columns.Add("x",typeof(double));
            table.Columns.Add("y",typeof(double));
           
        }
        public static void Open(MyAlog data,DataTable table)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "文本文件";
            open.Filter = "文本文件|*.txt";
            if(open.ShowDialog() == DialogResult.OK)
            {
                using(var sr=new StreamReader(open.FileName))
                {
                    sr.ReadLine();
                    int i = 0;
                    while(!sr.EndOfStream)
                    {
                        var bvr=sr.ReadLine().Split(' ');
                        MyPoint point=new MyPoint($"{i}", double.Parse(bvr[0]),double.Parse(bvr[1]));
                        
                        data.points.Add(point);
                        i++;
                    }
                }
                foreach (MyPoint point in data.points)
                {
                    DataRow rows = table.NewRow();
                    rows["id"] = point.Name;
                    rows["x"] = point.x;
                    rows["y"] = point.y;
                    table.Rows.Add(rows);
                }
            }
           
        }
        public static void Save(string data)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存文件";
            save.Filter = "文本文件|*.txt";
            if(save.ShowDialog() == DialogResult.OK)
            {
                using(var sr=new StreamWriter(save.FileName))
                {
                    sr.Write(data);
                }
            }
        }
        public static void Help()
        {
            MessageBox.Show("欢迎使用本程序");
        }
        public static void Draw_TB_WithVoronoi(MyAlog data, Chart sum)
        {
            // 清空图表
            sum.Series.Clear();

            // 原始数据点（蓝色）
            Series raw = new Series("原始数据点")
            {
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Blue,
                MarkerSize = 8
            };
            foreach (var p in data.points)
                raw.Points.AddXY(p.x, p.y);
            sum.Series.Add(raw);

            // 凸包点（红色）
            Series tb = new Series("凸包点")
            {
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Red,
                MarkerSize = 10
            };
            foreach (var p in data.TB)
                tb.Points.AddXY(p.x, p.y);
            sum.Series.Add(tb);

            // 绘制每个封闭泰森多边形（绿色折线）
            int index = 0;
            foreach (var kvp in data.VoronoiPolygons)
            {
                var centerPoint = kvp.Key;

                // 跳过凸包上的点（开放 Voronoi 图）
                if (data.TB.Contains(centerPoint)) continue;

                var polygon = kvp.Value;

                Series vor = new Series($"Voronoi_{index++}")
                {
                    ChartType = SeriesChartType.Line,
                    Color = System.Drawing.Color.Green,
                    BorderWidth = 2
                };

                // 添加折线点
                foreach (var p in polygon)
                    vor.Points.AddXY(p.x, p.y);

                // 封闭折线（回到起点）
                if (polygon.Count > 0)
                    vor.Points.AddXY(polygon[0].x, polygon[0].y);

                sum.Series.Add(vor);
            }

            // 设置坐标轴自动缩放
            sum.ChartAreas[0].AxisX.Minimum = data.points.Min(p => p.x) - 10;
            sum.ChartAreas[0].AxisX.Maximum = data.points.Max(p => p.x) + 10;
            sum.ChartAreas[0].AxisY.Minimum = data.points.Min(p => p.y) - 10;
            sum.ChartAreas[0].AxisY.Maximum = data.points.Max(p => p.y) + 10;

            sum.ChartAreas[0].AxisX.Title = "X";
            sum.ChartAreas[0].AxisY.Title = "Y";
        }
    }
}
