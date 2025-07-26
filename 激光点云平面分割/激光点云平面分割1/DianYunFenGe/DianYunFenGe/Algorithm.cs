using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DianYunFenGe
{
    class Algorithm
    {


        public static void caculate_main(ref DataCenter dc, ref RichTextBox richTextBox)
        {
            for (int i = 0; i < dc.points.Count; i++)
            {
                List<double> pos = Algorithm.caculate_posInsg(dc.points[i]);
                dc.points[i].position_i = pos[0];
                dc.points[i].position_j = pos[1];
            }
            int aa = 0;


            //统计坐标极大值极小值
            double x_max, x_min;
            double y_max, y_min;
            double z_max, z_min;

            x_max = dc.points_X.Max();
            x_min = dc.points_X.Min();

            y_max = dc.points_Y.Max();
            y_min = dc.points_Y.Min();

            z_max = dc.points_Z.Max();
            z_min = dc.points_Z.Min();



            //统计栅格单元C中的点云数目
            double num = Algorithm.caculate_dianShu(3, 2, dc);

            //计算栅格单元的平均高度
            double avg_H = Algorithm.caculate_Avg_height(3, 2, dc);

            double H_Cha = Algorithm.caculate_height_cha(3, 2, dc);

            double H_FangCha = Algorithm.caculate_height_fangCha(3, 2, dc);

            //计算面积与拟合平面
            double area = Algorithm.caculate_area(dc.points[0], dc.points[1], dc.points[2]);
            List<double> xishu = new List<double>();
            xishu = Algorithm.caculate_pingmian(dc.points[0], dc.points[1], dc.points[2]);


            //计算距离
            double dis = Algorithm.disOf_point_mian(dc.points[999], xishu);

            double dis1 = Algorithm.disOf_point_mian(dc.points[4], xishu);


            List<double> dis_all = new List<double>();

            for (int i = 0; i < dc.points.Count; i++)
            {
                double tem = Algorithm.disOf_point_mian(dc.points[i], xishu);
                dis_all.Add(tem);
            }

            double point_in = 0;
            double point_out = 0;
            for (int i = 0; i < dis_all.Count; i++)
            {
                if (dis_all[i] < 0.1)
                {
                    point_in++;
                }
                else
                {
                    point_out++;
                }
            }

            //计算最佳分割平面
            List<double> best = new List<double>();
            best = Algorithm.caculate_best_mian(dc);

            for(int i = 0; i < dc.points.Count; i++)
            {
              double dis_mm=  Algorithm.disOf_point_mian(dc.points[i], best);
                if (dis_mm < 0.1)
                {
                    dc.points[i].biaoshi="J1";
                }
                else
                {
                    dc.points[i].biaoshi = "unKnown";
                }

            }

            //计算第二次分割平面



            //输出报告
            richTextBox.Text = "点云平面分割结果\n";
            richTextBox.Text += "点号"+"\t"+ "X" + "\t"+ "Y" + "\t"+ "Z" + "\t"+ "标识" + "\n";
            for (int i = 0; i < dc.points.Count; i++)
            {
                richTextBox.Text += dc.points[i].pointName + "\t" + dc.points[i].X + "\t" + dc.points[i].Y + "\t" + dc.points[i].Z + "\t" + dc.points[i].biaoshi + "\n";
            }


            MessageBox.Show("计算成功，报告已经生成");

        }


        /// <summary>
        /// 计算点的位置
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static List<double> caculate_posInsg(Point point)
        {
            List<double> position = new List<double>();
            double i, j;
            i = Math.Floor(point.Y / 10);
            j = Math.Floor(point.X / 10);
            position.Add(i);
            position.Add(j);
            return position;
        }



        public static double caculate_dianShu(int m, int n, DataCenter dc)
        {
            double num = 0;
            for (int i = 0; i < dc.points.Count; i++)
            {
                if (dc.points[i].position_i == m)
                {
                    if (dc.points[i].position_j == n)
                    {
                        num++;
                    }

                }
            }
            return num;
        }

        public static double caculate_Avg_height(int m, int n, DataCenter dc)
        {
            double Z_sum = 0;
            double size = Algorithm.caculate_dianShu(m, n, dc);
            for (int i = 0; i < dc.points.Count; i++)
            {
                if (dc.points[i].position_i == m)
                {
                    if (dc.points[i].position_j == n)
                    {
                        Z_sum = Z_sum + dc.points[i].Z;
                    }

                }
            }
            return Z_sum / size;
        }


        public static double caculate_height_cha(int m, int n, DataCenter dc)
        {

            List<double> Z = new List<double>();

            for (int i = 0; i < dc.points.Count; i++)
            {
                if (dc.points[i].position_i == m)
                {
                    if (dc.points[i].position_j == n)
                    {
                        Z.Add(dc.points[i].Z);
                    }

                }
            }

            double max, min;
            max = Z.Max();
            min = Z.Min();
            return max - min;

        }

        public static double caculate_height_fangCha(int m, int n, DataCenter dc)
        {
            double tem = 0;
            double a = 0.0;
            double size = Algorithm.caculate_dianShu(m, n, dc);
            double avg = Algorithm.caculate_Avg_height(m, n, dc);
            for (int i = 0; i < dc.points.Count; i++)
            {
                if (dc.points[i].position_i == m)
                {
                    if (dc.points[i].position_j == n)
                    {
                        a = a + (avg - dc.points[i].Z) * (avg - dc.points[i].Z);
                    }

                }
            }

            tem = a / size;
            return tem;
        }

        public static double caculate_area(Point point1, Point point2, Point point3)
        {
            double area;
            double a = Algorithm.caculate_distance(point1, point2);
            double b = Algorithm.caculate_distance(point2, point3);
            double c = Algorithm.caculate_distance(point1, point3);

            double p = (a + b + c) / 2;
            area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return area;
        }



        public static double caculate_distance(Point point1, Point point2)
        {
            double dis = 0;
            dis = Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.Z - point2.Z) * (point1.Z - point2.Z));
            return dis;
        }


        public static List<double> caculate_pingmian(Point point1, Point point2, Point point3)
        {
            List<double> XiShu = new List<double>();

            double A, B, C, D;
            A = (point2.Y - point1.Y) * (point3.Z - point1.Z) - (point3.Y - point1.Y) * (point2.Z - point1.Z);
            B = (point2.Z - point1.Z) * (point3.X - point1.X) - (point3.Z - point1.Z) * (point2.X - point1.X);
            C = (point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y);
            D = -A * point1.X - B * point1.Y - C * point1.Z;

            XiShu.Add(A);
            XiShu.Add(B);
            XiShu.Add(C);
            XiShu.Add(D);

            return XiShu;
        }

        public static double disOf_point_mian(Point point, List<double> xishu)
        {
            double dis = 0;

            double A = xishu[0];
            double B = xishu[1];
            double C = xishu[2];
            double D = xishu[3];

            double b = Math.Abs(A * point.X + B * point.Y + C * point.Z + D);
            double a = Math.Sqrt(A * A + B * B + C * C);

            dis = b / a;
            return dis;
        }

        public static List<double> caculate_best_mian(DataCenter dc)
        {
            //List<double> XiShu = new List<double>();

            List<List<double>> XiShu = new List<List<double>>();
            List<double> In = new List<double>();
            List<double> Out = new List<double>();

            for (int i = 0; i < 900; i = i + 3)
            {
                List<double> dis_all = new List<double>();
                List<double> tem = new List<double>();
                tem = Algorithm.caculate_pingmian(dc.points[i], dc.points[i + 1], dc.points[i + 2]);

                XiShu.Add(tem);

                for (int j = 0; j < dc.points.Count; j++)
                {
                    if (j ==i)
                    {
                        continue;
                    }
                    if (j == i+1)
                    {
                        continue;
                    }
                    if (j == i+2)
                    {
                        continue;
                    }

                    double dis = Algorithm.disOf_point_mian(dc.points[j], tem);
                    dis_all.Add(dis);
                }

                double point_in = 0;
                double point_out = 0;
                for (int k = 0; k < dis_all.Count; k++)
                {
                    if (dis_all[k] < 0.1)
                    {
                        point_in++;
                    }

                }
                point_out = 997 - point_in;
                In.Add(point_in);
                Out.Add(point_out);
            }

            double b = In.Max();
            List<double> end_xishu = new List<double>();
            for(int t = 0; t < In.Count; t++)
            {
                if (In[t] == b)
                {
                    end_xishu = XiShu[t];
                }
            }

            return end_xishu;
        }

        public static List<double> caculate_best_mian2(DataCenter dc)
        {
            //List<double> XiShu = new List<double>();

            List<List<double>> XiShu = new List<List<double>>();
            List<double> In = new List<double>();
            List<double> Out = new List<double>();

            for (int i = 0; i < 900; i = i + 3)
            {
                List<double> dis_all = new List<double>();
                List<double> tem = new List<double>();
                tem = Algorithm.caculate_pingmian(dc.points[i], dc.points[i + 1], dc.points[i + 2]);

                XiShu.Add(tem);

                for (int j = 0; j < dc.points.Count; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    if (j == i + 1)
                    {
                        continue;
                    }
                    if (j == i + 2)
                    {
                        continue;
                    }

                    double dis = Algorithm.disOf_point_mian(dc.points[j], tem);
                    dis_all.Add(dis);
                }

                double point_in = 0;
                double point_out = 0;
                for (int k = 0; k < dis_all.Count; k++)
                {
                    if (dis_all[k] < 0.1)
                    {
                        point_in++;
                    }

                }
                point_out = 997 - point_in;
                In.Add(point_in);
                Out.Add(point_out);
            }

            double b = In.Max();
            List<double> end_xishu = new List<double>();
            for (int t = 0; t < In.Count; t++)
            {
                if (In[t] == b)
                {
                    end_xishu = XiShu[t];
                }
            }

            return end_xishu;
        }

    }
}
