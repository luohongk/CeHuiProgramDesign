using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QianFangJiaoHui
{
    class Algorithm
    {
        public static void caculate_XYZ(ref DataCenter dc,ref DataGridView dataGridView,ref RichTextBox richTextBox)
        {
            //计算像空间辅助坐标系
            List<double> X_list = new List<double>();
            List<double> Y_list= new List<double>();
            List<double> Z_list= new List<double>();

            for (int k = 0; k < dc.f.Count-1; k=k+2)
            {
                List<double> u = new List<double>();
                List<double> v = new List<double>();
                List<double> w = new List<double>();
                for (int i = 0; i < 2; i++)
                {

               
                        double x1 = dc.x[i+k];
                        double y1 = dc.y[i+k];
                        double f = dc.f[i+k];


                        double a1 = Math.Cos(dc.phi[i+k]) * Math.Cos(dc.k[i+k]) - Math.Sin(dc.phi[i+k]) * Math.Sin(dc.omega[i+k]) * Math.Sin(dc.k[i+k]);
                        double a2= -Math.Cos(dc.phi[i+k]) * Math.Cos(dc.k[i+k]) - Math.Sin(dc.phi[i+k]) * Math.Sin(dc.omega[i+k]) * Math.Sin(dc.k[i+k]);
                        double a3 = -Math.Sin(dc.phi[i+k]) * Math.Cos(dc.omega[i+k]);

                        double b1 = Math.Cos(dc.omega[i+k]) * Math.Sin(dc.k[i+k]);
                        double b2= Math.Cos(dc.omega[i+k]) * Math.Cos(dc.k[i+k]);
                        double b3 = -Math.Sin(dc.omega[i+k]);

                        double c1 = Math.Sin(dc.phi[i+k]) * Math.Cos(dc.k[i+k]) + Math.Cos(dc.phi[i+k]) * Math.Sin(dc.omega[i+k]) * Math.Sin(dc.k[i+k]);
                        double c2=-Math.Sin(dc.phi[i+k]) * Math.Cos(dc.k[i+k])+ Math.Cos(dc.phi[i+k]) * Math.Sin(dc.omega[i+k]) * Math.Sin(dc.k[i+k]);
                        double c3 = Math.Cos(dc.phi[i+k]) * Math.Cos(dc.omega[i+k]);

                        double u1 = a1 * x1 + a2 * y1 + a3 * (-f);
                        double v1 = b1 * x1 + b2 * y1 + b3 * (-f);
                        double w1 = c1 * x1 + c2 * y1 + c3 * (-f);

                        u.Add(u1);
                        v.Add(v1);
                        w.Add(w1);
                }
                //计算投影系数
                double Bu = dc.Xs[k + 1] - dc.Xs[k];
                double Bv= dc.Ys[k + 1] - dc.Ys[k];
                double Bw = dc.Zs[k + 1] - dc.Zs[k];

                double N1 = (Bu * w[1] - Bw * u[1]) / (u[0] * w[1] - u[1] * w[0]);
                double N2 = (Bu * w[0] - Bw * u[0]) / (u[0] * w[1] - u[1] * w[0]);

                double X = dc.Xs[k] + N1 * u[0];
                double Y = 0.5 * (dc.Ys[k] + N1 * v[0] + dc.Ys[k + 1] + N2 * v[1]);
                double Z = dc.Zs[k] + N1 * w[0];
                X_list.Add(X);
                Y_list.Add(Y);
                Z_list.Add(Z);

            }

            richTextBox.Text = "空间前方交会计算结果\n";
            richTextBox.Text += "****************************\n";
            richTextBox.Text += "点的序号\t" + "点的X\t" + "点的Y\t" + "点的Z\n";
            for(int i = 0; i < X_list.Count; i++)
            {
                richTextBox.Text += (i + 1).ToString() + "\t" + Math.Round(X_list[i], 3) + "\t" + Math.Round(Y_list[i], 3) + "\t" + Math.Round(Z_list[i], 3) + "\n";
            }

            MessageBox.Show("计算完成并且已经生成报告");

        }
    }
}
