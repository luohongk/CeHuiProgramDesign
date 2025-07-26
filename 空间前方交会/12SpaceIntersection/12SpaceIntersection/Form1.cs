using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Math;

namespace _12SpaceIntersection
{
    public partial class Form1 : Form
    {
        public double Xs1, Ys1, Zs1;
        public double Xs2, Ys2, Zs2;
        public double phi_1, omega_1, kappa_1;
        public double phi_2, omega_2, kappa_2;
        public double x1, y1, x2, y2;
        public double f;

        public double u1, v1, w1;
        public double u2, v2, w2;

        double N1, N2;
        public Form1()
        {
            InitializeComponent();
        }
        //获取数据
        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                string line01 = reader.ReadLine().Trim(); textBox01.Text = line01; Xs1 = Convert.ToDouble(line01);
                string line02 = reader.ReadLine().Trim(); textBox02.Text = line02; Ys1 = Convert.ToDouble(line02);
                string line03 = reader.ReadLine().Trim(); textBox03.Text = line03; Zs1 = Convert.ToDouble(line03);
                string line04 = reader.ReadLine().Trim(); textBox04.Text = line04; phi_1 = Convert.ToDouble(line04);
                string line05 = reader.ReadLine().Trim(); textBox05.Text = line05; omega_1 = Convert.ToDouble(line05);
                string line06 = reader.ReadLine().Trim(); textBox06.Text = line06; kappa_1 = Convert.ToDouble(line06);
                string line07 = reader.ReadLine().Trim(); textBox07.Text = line07; x1 = Convert.ToDouble(line07);
                string line08 = reader.ReadLine().Trim(); textBox08.Text = line08; y1 = Convert.ToDouble(line08);
                string line09 = reader.ReadLine().Trim(); textBox09.Text = line09; f = Convert.ToDouble(line09);
                string line10 = reader.ReadLine().Trim(); textBox10.Text = line10; Xs2= Convert.ToDouble(line10);
                string line11 = reader.ReadLine().Trim(); textBox11.Text = line11; Ys2 = Convert.ToDouble(line11);
                string line12 = reader.ReadLine().Trim(); textBox12.Text = line12; Zs2 = Convert.ToDouble(line12);
                string line13 = reader.ReadLine().Trim(); textBox13.Text = line13; phi_2 = Convert.ToDouble(line13);
                string line14 = reader.ReadLine().Trim(); textBox14.Text = line14; omega_2 = Convert.ToDouble(line14);
                string line15 = reader.ReadLine().Trim(); textBox15.Text = line15; kappa_2 = Convert.ToDouble(line15);
                string line16 = reader.ReadLine().Trim(); textBox16.Text = line16; x2 = Convert.ToDouble(line16);
                string line17 = reader.ReadLine().Trim(); textBox17.Text = line17; y2 = Convert.ToDouble(line17);
                textBox18.Text = line09;
            }
        }
        //计算空间辅助坐标
        private void button2_Click(object sender, EventArgs e)
        {
            double a1_1 = Cos(phi_1 / 180 * PI) * Cos(kappa_1 / 180 * PI) - Cos(phi_1 / 180 * PI) * Sin(omega_1 / 180 * PI) * Sin(kappa_1 / 180 * PI);
            double a1_2 = Cos(phi_2 / 180 * PI) * Cos(kappa_2 / 180 * PI) - Cos(phi_2 / 180 * PI) * Sin(omega_2 / 180 * PI) * Sin(kappa_2 / 180 * PI);

            double a2_1 = -Cos(phi_1 / 180 * PI) * Sin(kappa_1 / 180 * PI) - Sin(phi_1 / 180 * PI) * Sin(omega_1 / 180 * PI) * Sin(kappa_1 / 180 * PI);
            double a2_2 = -Cos(phi_2 / 180 * PI) * Sin(kappa_2 / 180 * PI) - Sin(phi_2 / 180 * PI) * Sin(omega_2 / 180 * PI) * Sin(kappa_2 / 180 * PI);

            double a3_1 = -Sin(phi_1 / 180 * PI) * Cos(omega_1 / 180 * PI);
            double a3_2 = -Sin(phi_2 / 180 * PI) * Cos(omega_2 / 180 * PI);

            double b1_1 = Cos(omega_1 / 180 * PI) * Sin(kappa_1 / 180 * PI);
            double b1_2 = Cos(omega_2 / 180 * PI) * Sin(kappa_2 / 180 * PI);

            double b2_1 = Cos(omega_1 / 180 * PI) * Cos(kappa_1 / 180 * PI);
            double b2_2 = Cos(omega_2 / 180 * PI) * Cos(kappa_2 / 180 * PI);

            double b3_1 = -Sin(omega_1 / 180 * PI);
            double b3_2 = -Sin(omega_2 / 180 * PI);

            double c1_1 = Sin(phi_1 / 180 * PI) * Cos(kappa_1 / 180 * PI) + Cos(phi_1 / 180 * PI) * Sin(omega_1 / 180 * PI) * Sin(kappa_1 / 180 * PI);
            double c1_2 = Sin(phi_2 / 180 * PI) * Cos(kappa_2 / 180 * PI) + Cos(phi_2 / 180 * PI) * Sin(omega_2 / 180 * PI) * Sin(kappa_2 / 180 * PI);

            double c2_1 = -Sin(phi_1/ 180 * PI) * Sin(kappa_1 / 180 * PI) + Cos(phi_1 / 180 * PI) * Sin(omega_1 / 180 * PI) * Cos(kappa_1 / 180 * PI);
            double c2_2 = -Sin(phi_2 / 180 * PI) * Sin(kappa_2 / 180 * PI) + Cos(phi_2 / 180 * PI) * Sin(omega_2 / 180 * PI) * Cos(kappa_2 / 180 * PI);

            double c3_1 = Cos(phi_1 / 180 * PI) * Cos(omega_1 / 180 * PI);
            double c3_2 = Cos(phi_2 / 180 * PI) * Cos(omega_2 / 180 * PI);

            u1 = a1_1 * x1 + a2_1 * y1 + a3_1 * (-f); textBox19.Text = u1.ToString();
            v1 = b1_1 * x1 + b2_1 * y1 + b3_1 * (-f); textBox20.Text = v1.ToString();
            w1 = c1_1 * x1 + c2_1 * y1 + c3_1 * (-f); textBox21.Text = w1.ToString();

            u2 = a1_2 * x2 + a2_2 * y2 + a3_2 * (-f); textBox22.Text = u2.ToString();
            v2 = b1_2 * x2 + b2_2 * y2 + b3_2 * (-f); textBox23.Text = v2.ToString();
            w2 = c1_2 * x2 + c2_2 * y2 + c3_2 * (-f); textBox24.Text = w2.ToString();
        }
        //获取投影系数
        private void button3_Click(object sender, EventArgs e)
        {
            double Bu = Xs2 - Xs1;
            double Bv = Ys2 - Ys1;
            double Bw = Zs2 - Zs1;

            N1 = (Bu * w2 - Bw * u2) / (u1 * w2 - u2 * w1); textBox25.Text = N1.ToString();
            N2 = (Bu * w1 - Bw * u1) / (u1 * w2 - u2 * w1); textBox26.Text = N2.ToString();


        }
        //计算地面坐标
        private void button4_Click(object sender, EventArgs e)
        {
            double X = Xs1 + N1 * u1; textBox27.Text = X.ToString();
            double Y = (Ys1 + N1 * v1 + Ys2 + N2 * v2) / 2; textBox28.Text = Y.ToString();
            double Z = Zs1 + N1 * w1; textBox29.Text = Z.ToString();
        }
    }
}
