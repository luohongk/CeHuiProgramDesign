using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaDIZhengFan
{
    class Algorithm
    {
        /// <summary>
        /// 坐标正算计算
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="richTextBox"></param>
        /// <param name="dataGridView"></param>
        public static void caculate_Zheng(ref DataCenter dc, ref RichTextBox richTextBox, ref DataGridView dataGridView)
        {
            richTextBox.Text += "中间过程数据结果\n";
            richTextBox.Text += "***************************\n";
            for (int i = 0; i < dc.start_point.Count; i++)
            {
                double f = 1 / dc.e_dao;
                double a = dc.a;
                double b = a * (1 - f);

                double e_2 = 1 - (b * b) / (a * a);
                double e_pie_2 = e_2 / (1 - e_2);
                double W1 = Math.Sqrt(1 - e_2 * Math.Sin(dc.B1[i]) * Math.Sin(dc.B1[i]));
                double sin_u1 = Math.Sin(dc.B1[i]) * Math.Sqrt(1 - e_2) / W1;
                double cos_u1 = Math.Cos(dc.B1[i]) / W1;

                //输出结果

                richTextBox.Text += "W1:" + Math.Round(W1, 3).ToString() + "\t" + "sin_u1:" + Math.Round(sin_u1, 3).ToString() + "\t" + "cos_u1:" + Math.Round(cos_u1, 3).ToString() + "\n";


                //计算辅助函数值
                double sin_A0 = cos_u1 * Math.Sin(dc.A12[i]);
                double cot_sigema1 = cos_u1 * Math.Cos(dc.A12[i]) / sin_u1;
                double sigema_1 = Math.Atan(1 / cot_sigema1);

                richTextBox.Text += "sin_A0:" + Math.Round(sin_A0, 3).ToString() + "\t" + "cot_sigema1:" + Math.Round(cot_sigema1, 3).ToString() + "\t" + "sigema1:" + Math.Round(Algorithm.huTodms(sigema_1), 3).ToString() + "\n";

                //计算A,B,C
                double cos_A0_2 = 1 - sin_A0 * sin_A0;
                double k_2 = e_pie_2 * cos_A0_2;
                double A = (1 - k_2 / 4 + 7 * k_2 * k_2 / 64 - 15 * k_2 * k_2 * k_2 / 256) / b;
                double B = k_2 / 4 - 7 * k_2 * k_2 / 8 + 37 * k_2 * k_2 * k_2 / 512;
                double C = k_2 * k_2 / 128 - k_2 * k_2 * k_2 / 128;

                double alpha = (e_2 / 2 + e_2 * e_2 / 8 + e_2 * e_2 * e_2 / 16) - (e_2 * e_2 / 16 + e_2 * e_2 * e_2 / 16) * cos_A0_2 + (3 * e_2 * e_2 * e_2 / 128) * cos_A0_2 * cos_A0_2;
                double beta = (e_2 * e_2 / 16 + e_2 * e_2 * e_2 / 16) * cos_A0_2 - e_2 * e_2 * e_2 / 32 * cos_A0_2 * cos_A0_2;
                double gama = e_2 * e_2 * e_2 / 256 * cos_A0_2 * cos_A0_2;

                double sigema = A * dc.S[i];
                double tem = 0;
                do
                {
                    tem = sigema;
                    sigema = A * dc.S[i] + B * Math.Sin(sigema) * Math.Cos(2 * sigema_1 + sigema) + C * Math.Sin(2 * sigema) * Math.Cos(4 * sigema_1 + 2 * sigema);

                }
                while (Math.Abs(tem - sigema) > Math.Pow(10, -10));

                //计算经差改正
                double delta = (alpha * sigema + beta * Math.Sin(sigema) * Math.Cos(2 * sigema_1 + sigema) + gama * Math.Sin(2 * sigema) * Math.Cos(4 * sigema_1 + 2 * sigema)) * sin_A0;

                //计算大地坐标与坐标方位角
                double sin_u2 = sin_u1 * Math.Cos(sigema) + cos_u1 * dc.A12[i] * Math.Sin(sigema);
                double B2 = Math.Atan(sin_u2 / (Math.Sqrt(1 - e_2) * Math.Sqrt(1 - sin_u2 * sin_u2)));
                double lameda = Math.Atan(Math.Sin(dc.A12[i]) * Math.Sin(sigema) / (cos_u1 * Math.Cos(sigema) - sin_u1 * Math.Sin(sigema) * Math.Cos(dc.A12[i])));

                double m, n;
                m = Math.Sin(dc.A12[i]);
                n = Math.Tan(lameda);

                if (m > 0)
                {
                    if (n > 0)
                    {
                        lameda = Math.Abs(lameda);
                    }
                    else
                    {
                        lameda = Math.PI - Math.Abs(lameda);
                    }
                }
                else
                {
                    if (n > 0)
                    {
                        lameda = Math.Abs(lameda) - Math.PI;
                    }
                    else
                    {
                        lameda = -Math.Abs(lameda);
                    }
                }

                double L2 = dc.L1[i] + lameda - delta;
                double A2 = Math.Atan(cos_u1 * Math.Sin(dc.A12[i]) / (cos_u1 * Math.Cos(sigema) * Math.Cos(dc.A12[i]) - sin_u1 * Math.Sin(sigema)));

                m = Math.Sin(dc.A12[i]);
                n = Math.Tan(A2);

                if (m > 0)
                {
                    if (n > 0)
                    {
                        A2 = Math.PI + Math.Abs(A2);
                    }
                    else
                    {
                        A2 = 2 * Math.PI - Math.Abs(A2);
                    }
                }
                else
                {
                    if (n > 0)
                    {
                        A2 = Math.Abs(A2);
                    }
                    else
                    {
                        A2 = Math.PI - Math.Abs(A2);
                    }
                }
                dc.B2.Add(B2);
                dc.L2.Add(L2);
                dc.A21.Add(A2);

                dataGridView.Rows[i].Cells[6].Value = Math.Round(Algorithm.huTodms(B2), 3);
                dataGridView.Rows[i].Cells[7].Value = Math.Round(Algorithm.huTodms(L2), 3);
                dataGridView.Rows[i].Cells[8].Value = Math.Round(Algorithm.huTodms(A2), 3);



            }

            MessageBox.Show("计算完成");


        }

        /// <summary>
        /// 坐标反算计算
        /// </summary>
        /// <param name="dc">数据中心</param>
        /// <param name="richTextBox">富文本</param>
        /// <param name="dataGridView">表格显示</param>
        ///      
        public static void caculate_Fan(ref DataCenter dc, ref RichTextBox richTextBox, ref DataGridView dataGridView)
        {
            for (int i = 0; i < dc.start_point.Count; i++)
            {
                double f = 1 / dc.e_dao;
                double a = dc.a;
                double b = a * (1 - f);

                double e_2 = 1 - (b * b) / (a * a);
                double e_pie_2 = e_2 / (1 - e_2);

                double u1 = Math.Atan(Math.Sqrt(1 - e_2) * Math.Tan(dc.B1[i]));
                double u2 = Math.Atan(Math.Sqrt(1 - e_2) * Math.Tan(dc.B2[i]));
                double l = dc.L2[i] - dc.L1[i];

                double a1 = Math.Sin(u1) * Math.Sin(u2);
                double a2 = Math.Cos(u1) * Math.Cos(u2);
                double b1 = Math.Cos(u1) * Math.Sin(u2);
                double b2 = Math.Sin(u1) * Math.Cos(u2);

                double delta = 0;
                double tem1, tem2;

                double lameda, A1, sigema, sin_A0,sigema1, cos_A0_2;

                do
                {
                    tem1 = delta;
                    lameda = l + delta;
                    double p = Math.Cos(u2) * Math.Sin(lameda);
                    double q = b1 - b2 * Math.Cos(lameda);
                    A1 = Math.Atan(p / q);

                    if (p > 0)
                    {
                        if (q > 0)
                        {
                            A1 = Math.Abs(A1);
                        }
                        else
                        {
                            A1 = Math.PI - Math.Abs(A1);
                        }
                    }
                    else
                    {
                        if (q < 0)
                        {
                            A1 = Math.PI + Math.Abs(A1);
                        }
                        else
                        {
                            A1 = 2 * Math.PI - Math.Abs(A1);
                        }
                    }

                    if (A1 < 0)
                    {
                        A1 = A1 + Math.PI * 2;
                    }

                    if (A1 > Math.PI * 2)
                    {
                        A1 = A1 - Math.PI * 2;
                    }

                    double sin_sigema = p * Math.Sin(A1) + q * Math.Cos(A1);
                    double cos_sigema = a1 + a2 * Math.Cos(lameda);
                    sigema = Math.Atan(sin_sigema / cos_sigema);
                    if (cos_sigema > 0)
                    {
                        sigema = Math.Abs(sigema);
                    }
                    else
                    {
                        sigema = Math.PI - Math.Abs(sigema);
                    }


                    sin_A0 = Math.Cos(u1) * Math.Sin(A1);
                     sigema1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));

                    //计算辅助量

                     cos_A0_2 = 1 - sin_A0 * sin_A0;
                    double alpha = (e_2 / 2 + e_2 * e_2 / 8 + e_2 * e_2 * e_2 / 16) - (e_2 * e_2 / 16 + e_2 * e_2 * e_2 / 16) * cos_A0_2 + (3 * e_2 * e_2 * e_2 / 128) * cos_A0_2 * cos_A0_2;
                    double beta = (e_2 * e_2 / 16 + e_2 * e_2 * e_2 / 16) * cos_A0_2 - e_2 * e_2 * e_2 / 32 * cos_A0_2 * cos_A0_2;
                    double gama = e_2 * e_2 * e_2 / 256 * cos_A0_2 * cos_A0_2;

                    delta = (alpha * sigema + beta * Math.Cos(2 * sigema1 + sigema) * Math.Sin(sigema) + gama * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema)) * sin_A0;

                    tem2 = delta;

                }
                while (Math.Abs(tem2 - tem1) * 206265 > 0.00001);

                //计算大地线长度

                //辅助；量A,B，C;
                cos_A0_2 = 1 - sin_A0 * sin_A0;
                double k_2 = e_pie_2 * cos_A0_2;
                double A = (1 - k_2 / 4 + 7 * k_2 * k_2 / 64 - 15 * k_2 * k_2 * k_2 / 256) / b;
                double B = k_2 / 4 - 7 * k_2 * k_2 / 8 + 37 * k_2 * k_2 * k_2 / 512;
                double C = k_2 * k_2 / 128 - k_2 * k_2 * k_2 / 128;

                delta = tem2;
                sigema1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));
                double x_s = C * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema);
                double S = (sigema - B * Math.Sin(sigema) * Math.Cos(2 * sigema1 + sigema) - x_s) / A;

                double A2 = Math.Atan((Math.Cos(u1) * Math.Sin(lameda)) / (b1 * Math.Cos(lameda) - b2));


                dataGridView.Rows[i].Cells[4].Value = S;
                dataGridView.Rows[i].Cells[3].Value = Algorithm.huTodms(A1);
                dataGridView.Rows[i].Cells[8].Value = Algorithm.huTodms(A2);


            }
        }

        /// <summary>
        /// 度分秒转弧度
        /// </summary>
        /// <param name="dms"></param>
        /// <returns></returns>
        public static double dmdTohd(double dms)
        {
            double hudu = 0.0;

            double d = Math.Floor(dms);
            double m = Math.Floor((dms - d) * 100);
            double s = ((dms - d) * 100 - m) * 100;

            hudu = (d + m / 60.0 + s / 3600.0) / 180 * Math.PI;
            return hudu;
        }


        /// <summary>
        /// 弧度转度分秒
        /// </summary>
        /// <param name="hudu"></param>
        /// <returns></returns>
        public static double huTodms(double hudu)
        {
            double dms = 0.0;
            double du = hudu / Math.PI * 180;

            double d = Math.Floor(du);

            double tem1 = du - d;
            double m = Math.Floor((tem1) * 60);

            double s = (tem1 * 60 - m) * 60;

            dms = s + m * 100 + d * 10000;
            return dms;
        }

    }
}
