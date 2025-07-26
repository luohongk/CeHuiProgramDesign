using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _11Curve_Fitting
{
    internal class Report
    {
        //原始数据表格
        public static void Open_Show(DataCenter data, DataGridView dataGridView)
        {
            foreach (Point p in data.points)
            {
                DataGridViewRow row = new DataGridViewRow();
                string[] values = { p.ID.ToString(), p.X.ToString(), p.Y.ToString() };
                foreach (string value in values)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = value;
                    row.Cells.Add(cell);
                }
                dataGridView.Rows.Add(row);
            }
        }
        //画图
        public static void Cal_Chart(List<Point> points, Chart chart, double interval, bool is_close)
        {
            chart.Series.Clear();

            Series series_points = new Series();//系列 1 ： 点
            series_points.ChartType = SeriesChartType.Point;
            chart.Series.Add(series_points);//这句话别忘了

            for(int i = 2; i < points.Count - 2; i++)
            {
                DataPoint point = new DataPoint(points[i].X, points[i].Y);
                point.Label = points[i].ID.ToString();
                series_points.Points.Add(point);
            }

            Series series_lines = new Series();//系列 2 ： 线
            series_lines.ChartType = SeriesChartType.Line;
            series_lines.MarkerStyle = MarkerStyle.None;
            chart.Series.Add(series_lines);

            if(is_close)
            {
                for (int i = 2; i < points.Count - 2; i++)//闭合拟合，包括最后一个点
                {
                    for (double z = 0; z <= 1; z = z + interval)//在两个已知点之间插入点来逼近曲线
                    {
                        double x = points[i].E0 + points[i].E1 * z + points[i].E2 * z * z + points[i].E3 * z * z * z;
                        double y = points[i].F0 + points[i].F1 * z + points[i].F2 * z * z + points[i].F3 * z * z * z;

                        DataPoint point = new DataPoint(x, y);
                        series_lines.Points.Add(point);
                    }
                }
            }
            else
            {
                for (int i = 2; i < points.Count - 3; i++)//不闭合拟合，不包括最后一个点
                {
                    for (double z = 0; z <= 1; z = z + interval)//在两个已知点之间插入点来逼近曲线
                    {
                        double x = points[i].E0 + points[i].E1 * z + points[i].E2 * z * z + points[i].E3 * z * z * z;
                        double y = points[i].F0 + points[i].F1 * z + points[i].F2 * z * z + points[i].F3 * z * z * z;

                        DataPoint point = new DataPoint(x, y);
                        series_lines.Points.Add(point);
                    }
                }
            }
        }
        //报告
        public static String Cal_Text(List<Point> points, bool is_close)
        {
            string text = "-------- 计算报告 --------\r\n\n";
            text += $"{"Start_ID",-10}{"End_ID",-10}{"E0",-8}{"E1",-8}{"E2",-8}{"E3",-8}{"F0",-8}{"F1",-8}{"F2",-8}{"F3",-8}\r\n";
            if(is_close)
            {
                for (int i = 2; i < points.Count - 2; i++)
                {
                    Point p = points[i];
                    text += $"{p.ID,-10}{points[i + 1].ID,-10}{p.E0,-8:f3}{p.E1,-8:f3}{p.E2,-8:f3}{p.E3,-8:f3}" +
                        $"{p.F0,-8:f3}{p.F1,-8:f3}{p.F2,-8:f3}{p.F3,-8:f3}\r\n";
                }
            }
            else
            {
                for (int i = 2; i < points.Count - 3; i++)
                {
                    Point p = points[i];
                    text += $"{p.ID,-10}{points[i + 1].ID,-10}{p.E0,-8:f3}{p.E1,-8:f3}{p.E2,-8:f3}{p.E3,-8:f3}" +
                        $"{p.F0,-8:f3}{p.F1,-8:f3}{p.F2,-8:f3}{p.F3,-8:f3}\r\n";
                }
            }

            return text;
        }
    }
}
