using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Algorithm
    {
        public static void positiveDataSolution(ref DataCenter dc, ref FileCenter fc, ref DataGridView dataGridView1)
        {

            DataCenter.initial(dc);
            //数据导入
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dc.B1.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value)));
                    dc.L1.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value)));
                    dc.A1.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value)));
                    dc.S.Add(Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value));
                    dc.A2.Add(0);
                    dc.B2.Add(0);
                    dc.L2.Add(0);

                }
            }
            catch
            {
                MessageBox.Show("请输入正确的数据");
            }

            #region 正算核心

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                //计算起点归化经度
                double w1 = Math.Sqrt(1 - Math.Pow(dc.e1, 2) * Math.Pow(Math.Sin(dc.B1[i]), 2));
                double sinu1 = (Math.Sin(dc.B1[i]) * Math.Sqrt(1 - Math.Pow(dc.e1, 2))) / w1;
                double cosu1 = Math.Cos(dc.B1[i]) / w1;
                //计算辅助函数值
                double sinA0 = cosu1 * Math.Sin(dc.A1[i]);
                double cotsigema1 = cosu1 * Math.Cos(dc.A1[i]) / sinu1;
                double sigema1 = Math.Atan(1 / cotsigema1);
                //计算系数A,B,C以及alpha,Beita,gama
                double cosA0_2 = 1 - sinA0 * sinA0;
                double e2_2 = Math.Pow(dc.e2, 2);
                double k_2 = e2_2 * cosA0_2;
                double A = (1 - (k_2 / 4) + (7 * k_2 * k_2 / 64) - (15 * k_2 * k_2 * k_2) / 256) / dc.b;
                double B = (k_2 / 4 - 7 * k_2 * k_2 / 8 + 37 * k_2 * k_2 * k_2 / 512);
                double C = k_2 * k_2 / 128 - k_2 * k_2 * k_2 / 128;

                //计算球面长度（迭代）
                double sigema = A * dc.S[i];
                double sigema_diedai, delta_sigema;
                do
                {
                    sigema_diedai = A * dc.S[i] + B * Math.Sin(sigema) * Math.Cos(2 * sigema1 + sigema) + C * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema);
                    delta_sigema = sigema_diedai - sigema;
                    sigema = sigema_diedai;

                }
                while (delta_sigema > Math.Pow(10, -10));


                //计算经差改正
                double e1_2 = Math.Pow(dc.e1, 2);
                double alpha = ((e1_2 / 2) + (e1_2 * e1_2 / 8) + (e1_2 * e1_2 * e1_2 / 16)) - (e1_2 * e1_2 / 16 + Math.Pow(e1_2, 3) / 16) * cosA0_2 + (3 * Math.Pow(e1_2, 3) / 128) * cosA0_2 * cosA0_2;
                double beita = (Math.Pow(e1_2, 2) / 16 + Math.Pow(e1_2, 4) / 16) * cosA0_2 - (Math.Pow(e1_2, 3) / 32) * cosA0_2 * cosA0_2;
                double gama = (Math.Pow(e1_2, 3) / 256) * cosA0_2 * cosA0_2;

                double delta_L = (alpha * sigema + beita * Math.Sin(sigema) * Math.Cos(2 * sigema1 + sigema) + gama * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema)) * sinA0;


                //计算终点的大地坐标与大地方位角
                double sinu_2 = sinu1 * Math.Cos(sigema_diedai) + cosu1 * Math.Cos(dc.A1[i]) * Math.Sin(sigema_diedai);
                dc.B2[i] = Math.Atan(sinu_2 / (Math.Sqrt(1 - dc.e1 * dc.e1) * Math.Sqrt(1 - sinu_2 * sinu_2)));
                double lameda = Math.Atan(Math.Sin(dc.A1[i]) * Math.Sin(sigema_diedai) / (cosu1 * Math.Cos(sigema_diedai) - sinu1 * Math.Sin(sigema_diedai) * Math.Cos(dc.A1[i])));

                double a = Math.Sin(dc.A1[i]);
                double b = Math.Tan(lameda);
                if (a > 0)
                {
                    if (b > 0)
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
                    if (b < 0)
                    {
                        lameda = -1 * Math.Abs(lameda);
                    }
                    else
                    {
                        lameda = Math.Abs(lameda) - Math.PI;
                    }
                }
                dc.L2[i] = dc.L1[i] + lameda - delta_L;
                dc.A2[i] = Math.Atan((cosu1 * Math.Sin(dc.A1[i])) / (cosu1 * Math.Cos(sigema_diedai) * Math.Cos(dc.A1[i]) - sinu1 * Math.Sin(sigema_diedai)));

                double c = Math.Tan(dc.A2[i]);
                if (a < 0)
                {
                    if (c > 0)
                    {
                        dc.A2[i] = Math.Abs(dc.A2[i]);
                    }
                    else
                    {
                        dc.A2[i] = Math.PI - Math.Abs(dc.A2[i]);
                    }
                }
                else
                {
                    if (c > 0)
                    {
                        dc.A2[i] = Math.PI + Math.Abs(dc.A2[i]);
                    }
                    else
                    {
                        dc.A2[i] = 2 * Math.PI - Math.Abs(dc.A2[i]);
                    }
                }
                #region
                dataGridView1.Rows[i].Cells[6].Value = DataCenter.hutudms(dc.B2[i]);
                dataGridView1.Rows[i].Cells[7].Value = DataCenter.hutudms(dc.L2[i]);
                dataGridView1.Rows[i].Cells[8].Value = DataCenter.hutudms(dc.A2[i]);
                #endregion

            }

            #endregion

        }

        public static void invSolution(ref DataCenter dc, ref FileCenter fc, ref DataGridView dataGridView1)
        {
            //数据初始化
            DataCenter.initial(dc);
            //将datagridview1的数据传入到dc对象
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    //传数据的时候一定都要转换成弧度
                    dc.B1.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value)));
                    dc.L1.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value)));
                    dc.B2.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value)));
                    dc.L2.Add(DataCenter.dmstohd(Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value)));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #region 大地线长度计算
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                //辅助计算量
                double u1 = Math.Atan(Math.Sqrt(1 - dc.e1 * dc.e1) * Math.Tan(dc.B1[i]));
                double u2 = Math.Atan(Math.Sqrt(1 - dc.e1 * dc.e1) * Math.Tan(dc.B2[i]));
                double l = dc.L2[i] - dc.L1[i];
                double a1 = Math.Sin(u1) * Math.Sin(u2);
                double a2 = Math.Cos(u1) * Math.Cos(u2);
                double b1 = Math.Cos(u1) * Math.Sin(u2);
                double b2 = Math.Sin(u1) * Math.Cos(u2);

                //计算起点的大地方位角，采用逐次趋同法计算大地方位角
                double delta = 0, delta_cha, sigema1, sigema, A1, cosA0_2, e2_2, lameda;
                do
                {
                    //每一次迭代起始delta
                    double delta0 = delta;
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
                        if (q > 0)
                        {
                            A1 = 2 * Math.PI - Math.Abs(A1);
                        }
                        else
                        {
                            A1 = Math.PI + Math.Abs(A1);
                        }
                    }
                    if (A1 < 0)
                    {
                        A1 = A1 + 2 * Math.PI;

                    }
                    if (A1 > 2 * Math.PI)
                    {
                        A1 = A1 - 2 * Math.PI;
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

                    double sinA0 = Math.Cos(u1) * Math.Sin(A1);
                    sigema1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));

                    double sin_A0 = Math.Cos(u1) * Math.Sin(A1);
                    cosA0_2 = 1 - sinA0 * sinA0;


                    double e1_2 = dc.e1 * dc.e1;
                    e2_2 = dc.e2 * dc.e2;
                    double alpha = ((e1_2 / 2) + (e1_2 * e1_2 / 8) + (e1_2 * e1_2 * e1_2 / 16)) - (e1_2 * e1_2 / 16 + Math.Pow(e1_2, 3) / 16) * cosA0_2 + (3 * Math.Pow(e1_2, 3) / 128) * cosA0_2 * cosA0_2;
                    double beita = (Math.Pow(e1_2, 2) / 16 + Math.Pow(e1_2, 4) / 16) * cosA0_2 - (Math.Pow(e1_2, 3) / 32) * cosA0_2 * cosA0_2;
                    double gama = (Math.Pow(e1_2, 3) / 256) * cosA0_2 * cosA0_2;

                    delta = (alpha * sigema + beita * Math.Cos(2 * sigema1 + sigema) * Math.Sin(sigema) + gama * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema)) * sinA0;
                    delta_cha = Math.Abs(delta - delta0);


                }
                while (delta_cha > Math.Pow(10, -10));
                double k_2 = e2_2 * cosA0_2;
                double A = (1 - (k_2 / 4) + (7 * k_2 * k_2 / 64) - (15 * k_2 * k_2 * k_2) / 256) / dc.b;
                double B = (k_2 / 4 - 7 * k_2 * k_2 / 8 + 37 * k_2 * k_2 * k_2 / 512);
                double C = k_2 * k_2 / 128 - k_2 * k_2 * k_2 / 128;

                //计算大地线的长度
                sigema1 = Math.Atan(Math.Tan(u1) / Math.Cos(A1));

                double xs = C * Math.Sin(2 * sigema) * Math.Cos(4 * sigema1 + 2 * sigema);
                double S = (sigema - B * Math.Sin(sigema) * Math.Cos(2 * sigema1 + sigema) - xs) / A;

                //计算反方位角A2;
                double A2 = Math.Atan((Math.Cos(u1) * Math.Sin(lameda)) / (b1 * Math.Cos(lameda) - b2));

                if (A2 < 0)
                {
                    A2 = A2 + Math.PI * 2;
                }
                if (A2 > Math.PI * 2)
                {
                    A2 = A2 - Math.PI * 2;
                }
                if (A1 < Math.PI && A2 < Math.PI)
                {
                    A2 = A2 + Math.PI;
                }
                if (A1 > Math.PI && A2 > Math.PI)
                {
                    A2 = A2 - Math.PI;
                }
                //将计算结果写入datagridview
                dataGridView1.Rows[i].Cells[3].Value = Math.Round(DataCenter.hutudms(A1), 5);
                dataGridView1.Rows[i].Cells[4].Value = Math.Round(S, 3);
                dataGridView1.Rows[i].Cells[8].Value = Math.Round(DataCenter.hutudms(A2), 5);


            }




            #endregion





        }
    }
}
