using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpatialDataAnalysis
{
    class Algorithm
    {
        public static double[,] WeightMatrix;
        /// <summary>
        /// 计算标准差椭圆
        /// </summary>
        /// <param name="data"></param>
        /// <param name="chart1"></param>
        /// <returns></returns>
        public static string CalEllipse(DataCenter data,Chart chart1)
        {
            string text = null;

            for(int i = 1; i < 8; i++)
            {
                Area a = new Area();
                a.Code = i;
                data.Areas.Add(a);
            }
            //数据统计
            foreach(Point p in data.Points)
            {
                for(int i = 1; i < 8; i++)
                {
                    if (p.AreaCode == i)
                    {
                        data.Areas[i - 1].AreaPoints.Add(p);
                        data.Areas[i - 1].PointsNum++;
                        continue;
                    }

                }                
            }
            int n = 0;
            foreach(Area a in data.Areas)
            {
                n += a.PointsNum;
            }
            //计算平均中心
            double XAver = data.Points.Sum(p => p.X / n);
            double YAver = data.Points.Sum(p => p.Y / n);
            //计算标准差椭圆
            double sum1 = data.Points.Sum(p => Pow(p.X - XAver, 2));
            double sum2 = data.Points.Sum(p => Pow(p.Y - YAver, 2));
            double sum3 = data.Points.Sum(p => (p.Y - YAver) * (p.X - XAver));
            double A = sum1 - sum2;
            double B = Sqrt(Pow(sum1 - sum2, 2) + 4 * sum3 * sum3);
            double C = 2 * sum3;

            double theta = Atan((A + B) / C);
            double up1 = data.Points.Sum(p => Pow((p.X - XAver) * Cos(theta) + (p.Y - YAver) * Sin(theta), 2));
            double SDEx = Sqrt((2 * up1) / n);
            double up2 = data.Points.Sum(p => Pow((p.X - XAver) * Sin(theta) - (p.Y - YAver) * Cos(theta), 2));
            double SDEy = Sqrt((2 * up2) / n);
            //绘图
            chart1.Series.Clear();
            Series s1 = new Series();//创建系列
            s1.Name = "事件点";
            for (int i = 0; i < data.Points.Count; i++)
            {
                DataPoint p = new DataPoint(data.Points[i].X, data.Points[i].Y);//创建数据点（X, Y）
                s1.Points.Add(p);//数据点加入系列
            }
            s1.ChartType = SeriesChartType.Point;
            s1.MarkerStyle = MarkerStyle.Circle;
            chart1.Series.Add(s1);//系列加入图表

            Series s2 = new Series();//创建系列
            s2.Name = "标准差椭圆";
            for (double theta0 = 0; theta0 <= 2 * PI; theta0 += 0.1)
            {
                double x = XAver + SDEx * Cos(theta0) * Cos(theta) - SDEy * Sin(theta0) * Sin(theta);
                double y = YAver + SDEx * Cos(theta0) * Sin(theta) - SDEy * Sin(theta0) * Cos(theta);
                DataPoint dp = new DataPoint(x, y);
                s2.Points.Add(dp);
            }
            s2.ChartType = SeriesChartType.Line;
            //2.MarkerStyle = MarkerStyle.Circle;
            chart1.Series.Add(s2);//系列加入图表

            text += $"{data.Number}，1 区（区号为 1）的事件数量 n1，{data.Areas[0].PointsNum}\r\n"; data.Number++;
            text += $"{data.Number}，4 区（区号为 4）的事件数量 n4，{data.Areas[3].PointsNum}\r\n"; data.Number++;
            text += $"{data.Number}，6 区（区号为 6）的事件数量 n6，{data.Areas[5].PointsNum}\r\n"; data.Number++;
            text += $"{data.Number}，事件总数 n，{n}\r\n"; data.Number++;
            text += $"{data.Number}，坐标分量 x 的平均值，{XAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，坐标分量 y 的平均值，{YAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，P6 坐标分量与平均中心之间的偏移量 a6，{data.Points[5].X - XAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，P6 坐标分量与平均中心之间的偏移量 b6，{data.Points[5].Y - YAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，辅助量 A，{A:f3}\r\n"; data.Number++;
            text += $"{data.Number}，辅助量 B，{B:f3}\r\n"; data.Number++;
            text += $"{data.Number}，辅助量 C，{C:f3}\r\n"; data.Number++;
            text += $"{data.Number}，标准差椭圆长轴与竖直方向的夹角，{theta:f3}\r\n"; data.Number++;
            text += $"{data.Number}，标准差椭圆的长半轴，{SDEx:f3}\r\n"; data.Number++;
            text += $"{data.Number}，标准差椭圆的短半轴，{SDEy:f3}\r\n"; data.Number++;

            return text;
        }

        /// <summary>
        /// 计算空间权重矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CalWeightMatrix(DataCenter data)
        {
            string text = null;

            //计算各区平均中心
            foreach(Area a in data.Areas)
            {
                a.AxAver = a.AreaPoints.Sum(p => p.X / a.PointsNum);
                a.AyAver = a.AreaPoints.Sum(p => p.Y / a.PointsNum);
            }
            //计算空间权重矩阵
            WeightMatrix = new double[7, 7];
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    if (i == j) WeightMatrix[i, j] = 0.0;
                    else
                    {
                        WeightMatrix[i, j] = 1000.0 / Sqrt(Pow(data.Areas[i].AxAver - data.Areas[j].AxAver, 2) 
                            + Pow(data.Areas[i].AyAver - data.Areas[j].AyAver, 2));
                    }
                }
            }           

            text += $"{data.Number}，1 区平均中心的坐标分量 X，{data.Areas[0].AxAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，1 区平均中心的坐标分量 Y，{data.Areas[0].AyAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，4 区平均中心的坐标分量 X，{data.Areas[3].AxAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，4 区平均中心的坐标分量 Y，{data.Areas[3].AyAver:f3}\r\n"; data.Number++;
            text += $"{data.Number}，1 区和 4 区的空间权重，{WeightMatrix[0, 3]:f6}\r\n"; data.Number++;
            text += $"{data.Number}，6 区和 7 区的空间权重，{WeightMatrix[5, 6]:f6}\r\n"; data.Number++;

            return text;
        }

        /// <summary>
        /// 计算莫兰系数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CalMolanIndex(DataCenter data)
        {
            string text = null;

            //数据整理
            int n = 0;
            foreach (Area a in data.Areas)
            {
                n += a.PointsNum;
            }
            double XAve = n / 7.0;
            //全局莫兰指数
            double S0 = 0.0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    S0 += WeightMatrix[i, j];
                }
            }
            double down = data.Areas.Sum(p => Pow(p.PointsNum - XAve, 2));
            double up = 0.0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    up += WeightMatrix[i, j] * (data.Areas[i].PointsNum - XAve) * (data.Areas[j].PointsNum - XAve);
                }
            }
            double I = (7.0 * up) / (S0 * down);

            //局部莫兰指数
            for(int i = 0; i < 7; i++)
            {
                double Si_2 = 0.0;
                for (int j = 0; j < 7; j++)
                {
                    if (i == j) continue;
                    else
                    {
                        Si_2 += Pow(data.Areas[j].PointsNum - XAve, 2);
                    }
                }
                Si_2 /= 6.0;

                double sum = 0.0;
                for (int j = 0; j < 7; j++)
                {
                    if (i == j) continue;
                    else
                    {
                        sum += WeightMatrix[i, j] * (data.Areas[j].PointsNum - XAve);
                    }
                }
                data.Areas[i].I = (data.Areas[i].PointsNum - XAve) * sum / Si_2;
            }
            //计算局部莫兰指数的 Z 得分
            double u = data.Areas.Sum(p => p.I / 7.0);
            double sum0 = data.Areas.Sum(p => Pow(p.I - u, 2));
            double sigma = Sqrt(sum0 / 6.0);
            foreach(Area a in data.Areas)
            {
                a.Z = (a.I - u) / sigma;
            }

            text += $"{data.Number}，研究区域犯罪事件的平均值，{XAve:f6}\r\n"; data.Number++;
            text += $"{data.Number}，全局莫兰指数辅助量 S0，{S0:f6}\r\n"; data.Number++;
            text += $"{data.Number}，全局莫兰指数 I，{I:f6}\r\n"; data.Number++;

            text += $"{data.Number}，1 区的局部莫兰指数 I1，{data.Areas[0].I:f6}\r\n"; data.Number++;
            text += $"{data.Number}，3 区的局部莫兰指数 I3，{data.Areas[2].I:f6}\r\n"; data.Number++;
            text += $"{data.Number}，5 区的局部莫兰指数 I5，{data.Areas[4].I:f6}\r\n"; data.Number++;
            text += $"{data.Number}，7 区的局部莫兰指数 I7，{data.Areas[6].I:f6}\r\n"; data.Number++;
            text += $"{data.Number}，局部莫兰指数的平均数，{u:f6}\r\n"; data.Number++;
            text += $"{data.Number}，局部莫兰指数的标准差，{sigma:f6}\r\n"; data.Number++;
            text += $"{data.Number}，1 区局部莫兰指数的 Z 得分 Z1，{data.Areas[0].Z:f6}\r\n"; data.Number++;
            text += $"{data.Number}，3 区局部莫兰指数的 Z 得分 Z3，{data.Areas[2].Z:f6}\r\n"; data.Number++;
            text += $"{data.Number}，5 区局部莫兰指数的 Z 得分 Z5，{data.Areas[4].Z:f6}\r\n"; data.Number++;
            text += $"{data.Number}，7 区局部莫兰指数的 Z 得分 Z7，{data.Areas[6].Z:f6}\r\n"; data.Number++;


            return text;
        }
    }
}
