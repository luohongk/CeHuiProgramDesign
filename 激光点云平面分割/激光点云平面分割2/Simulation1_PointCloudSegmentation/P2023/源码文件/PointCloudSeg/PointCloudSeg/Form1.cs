using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointCloudSeg
{
    public partial class Form1 : Form
    {
        int pointnum = 0;
        List<Pointinfo> originpoints;//点云数据
        ClusterPoint[,] cluster = new ClusterPoint[10, 10];//栅格
        string report;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 菜单栏——导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmImportData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                originpoints = new List<Pointinfo>();
                StreamReader sr = new StreamReader(ofd.FileName);
                string line;
                string[] parts;

                double xmin = 1000, xmax = -1000;
                double ymin = 1000, ymax = -1000;
                double zmin = 1000, zmax = -1000;

                line = sr.ReadLine();
                pointnum = int.Parse(line);

                //读取点数据
                while ((line = sr.ReadLine()) != null)
                {
                    parts = line.Split(',');
                    double x = double.Parse(parts[1]);
                    double y = double.Parse(parts[2]);
                    double z = double.Parse(parts[3]);
                    Pointinfo p = new Pointinfo(
                        parts[0],x,y,z);

                    if (x < xmin) xmin = x;
                    if (x > xmax) xmax = x;
                    if (y < ymin) ymin = y;
                    if (y > ymax) ymax = y;
                    if (z < zmin) zmin = z;
                    if (z > zmax) zmax = z;

                    originpoints.Add(p);

                }
                //zmax = Cal.GetZmax(originpoints);
                if (pointnum != originpoints.Count)
                {
                    MessageBox.Show("数据点数不对！");
                }

                string text = "\n导入数据：\n";
                text += String.Format("{0,-20}{1,-20}", "P5的坐标分量 x", originpoints[4].X) + "\n";
                text += String.Format("{0,-20}{1,-20}", "P5的坐标分量 y", originpoints[4].Y) + "\n";
                text += String.Format("{0,-20}{1,-20}", "P5的坐标分量 z", originpoints[4].Z) + "\n";

                text += String.Format("{0,-20}{1,-20}", "坐标分量x的最小值xmin", xmin) + "\n";
                text += String.Format("{0,-20}{1,-20}", "坐标分量x的最大值 xmax", xmax) + "\n";

                text += String.Format("{0,-20}{1,-20}", "坐标分量y的最小值ymin", ymin) + "\n";
                text += String.Format("{0,-20}{1,-20}", "坐标分量y的最大值 ymax", ymax) + "\n";

                text += String.Format("{0,-20}{1,-20}", "坐标分量z的最小值zmin", zmin) + "\n";
                text += String.Format("{0,-20}{1,-20}", "坐标分量z的最大值zmax", zmax) + "\n";

                richTextBox1.AppendText(text);
                report += text;
                tabControl1.SelectedIndex = 0;
                MessageBox.Show("数据导入成功！");
            }
        }

        /// <summary>
        /// 工具栏——导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbImportData_Click(object sender, EventArgs e)
        {
            tsmImportData_Click(sender, e);
        }

        /// <summary>
        /// 菜单栏——保存报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSaveReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt files|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, report);
                MessageBox.Show("计算报告保存成功！");
            }
        }

        /// <summary>
        /// 工具栏——保存报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSaveReport_Click(object sender, EventArgs e)
        {
            tsmSaveReport_Click(sender, e);
        }

        /// <summary>
        /// 菜单栏——栅格化以及相关计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCalCluster_Click(object sender, EventArgs e)
        {
            if (originpoints == null)
            {
                MessageBox.Show("请先导入数据！");
                return;
            }
            
            for(int i=0;i<10;i++)//初始化
                for(int j = 0; j < 10; j++)
                {
                    cluster[i, j] = new ClusterPoint();
                }

            double dx = 10, dy = 10;
            for(int cnt=0; cnt < originpoints.Count; cnt++)
            {
                int i =(int)( originpoints[cnt].X / dx);
                int j = (int)(originpoints[cnt].Y / dy);

                originpoints[cnt].labeli = i;
                originpoints[cnt].labelj = j;
                
                cluster[i, j].Points.Add(originpoints[cnt]);
            }
            string text = "\n栅格化以及相关计算：\n";
            text += String.Format("{0,-20}{1,-20}", "P5 点的所在栅格的行i", originpoints[4].labeli) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P5 点的所在栅格的列j", originpoints[4].labelj) + "\n";
            text += String.Format("{0,-20}{1,-20}", "栅格C 中的点的数量", cluster[2, 3].Points.Count) + "\n";

            text += String.Format("{0,-20}{1,-20}", "栅格C 中的平均高度",Cal.GetAveH( cluster[2, 3].Points).ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "栅格C 中高度的最大值", Cal.GetZmax(cluster[2, 3].Points).ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "栅格C 中的高度差", Cal.GetDiffH(cluster[2, 3].Points).ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "栅格C 中的高度方差", Cal.GetS2(cluster[2, 3].Points).ToString("0.000")) + "\n";

            richTextBox1.AppendText(text);
            report += text;
            tabControl1.SelectedIndex = 0;
            MessageBox.Show("栅格化以及相关计算成功！" );
        }

        /// <summary>
        /// 工具栏——栅格化以及相关计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCalCluster_Click(object sender, EventArgs e)
        {

            tsmCalCluster_Click(sender, e);
        }

        /// <summary>
        /// 菜单栏——平面分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSeg_Click(object sender, EventArgs e)
        {
            if (originpoints == null)
            {
                MessageBox.Show("请先导入数据！");
                return;
            }
            //if (cluster[0, 0] == null)
            //{
            //    MessageBox.Show("请先栅格化！");
            //    return;
            //}

            //平面拟合
            double area = Segment.GetArea(originpoints[0], originpoints[1], originpoints[2]);
            double []ABCD=Segment.GetABCD(originpoints[0], originpoints[1], originpoints[2]);
           
            string text = "\n平面分割：\n";
            text += "\n平面拟合：\n";
            text += String.Format("{0,-20}{1,-20}", "P1-P2-P3 构成三角形的面积",area.ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的参数A", ABCD[0].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的参数B", ABCD[1].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的参数C", ABCD[2].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的参数D", ABCD[3].ToString("0.000000")) + "\n";

            //内部点和外部点计算
            double d1000 = Segment.GetP2S(originpoints[999], ABCD);
            double d5 = Segment.GetP2S(originpoints[4], ABCD);
            text += "\n平面拟合：\n";
            text += String.Format("{0,-20}{1,-20}", "P1000 到拟合平面S1 的距离", d1000.ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P5 到拟合平面S1 的距离", d5.ToString("0.000")) + "\n";

            double inner=0, outer = 0;//内部点和外部点数量
            for (int i = 0; i < originpoints.Count; i++)
            {
                double dist = Segment.GetP2S(originpoints[i], ABCD);
                if (dist < 0.1)
                {
                    originpoints[i].J1 = 1;
                    inner++;
                }
                else
                {
                    originpoints[i].J1 = 0;
                    outer++;
                }
            }

            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的内部点数量", (inner-3).ToString()) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面S1 的外部点数量", outer.ToString()) + "\n";

            //最佳分割平面计算
            double innermax = inner, outermin = 0;
            double[] ABCDJ1=ABCD;

            for (int i = 0; i < 331; i++)
            {
                double inner1 = 0, outer1 = 0;
                ABCD = Segment.GetABCD(originpoints[(3 * i)], originpoints[(3 * i + 1)], originpoints[(3 * i + 2)]);
                for (int j = 0; j < originpoints.Count; j++)
                {                
                    double dist = Segment.GetP2S(originpoints[j], ABCD);
                    if (dist < 0.1)
                    {
                        originpoints[j].J1 = 1;
                        inner1++;
                    }
                    else
                    {
                        originpoints[j].J1 = 0;
                        outer1++;
                    }
                }
                //MessageBox.Show(inner1.ToString());
                if (inner1 > innermax)
                {
                    ABCDJ1 = ABCD;
                    innermax = inner1;

                    outermin = outer1;
                }
            }

            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的参数A", ABCDJ1[0].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的参数B", ABCDJ1[1].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的参数C", ABCDJ1[2].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的参数D", ABCDJ1[3].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的内部点数量", (innermax - 3).ToString()) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J1 的外部点数量", outermin.ToString()) + "\n";

            //迭代计算平面分割
            List<Pointinfo> newpoints = new List<Pointinfo>();
            for(int i = 0; i < originpoints.Count; i++)
            {
                newpoints.Add(originpoints[i]);
            }

            for (int i = 0; i < newpoints.Count; i++)//除最佳分割面J1 的内部点、及拟合J1 平面的所用的三个点
            {
                double dist = Segment.GetP2S(newpoints[i], ABCDJ1);
                if (dist < 0.1)
                {
                    newpoints[i].J1 = 1;
                }
                else
                {
                    newpoints[i].J1 = 0;
                }
            }
            newpoints.RemoveAll(p => p.J1 != 0);

            double innermax2 =0, outermin2 = 0;
            double[] ABCDJ2=ABCD ;

            for (int i = 0; i < 80; i++)//迭代
            {
                double inner2 = 0, outer2 = 0;
                ABCD = Segment.GetABCD(newpoints[3 * i], newpoints[3 * i + 1], newpoints[3 * i + 2]);
                for (int j = 0; j < newpoints.Count; j++)
                {
                    double dist = Segment.GetP2S(newpoints[j], ABCD);
                    if (dist < 0.1)
                    {
                        newpoints[j].J1 = 1;
                        inner2++;
                    }
                    else
                    {
                        newpoints[j].J1 = 0;
                        outer2++;
                    }
                }
                if (inner2 > innermax2)
                {
                    ABCDJ2 = ABCD;
                    innermax2 = inner2;
                    outermin2 = outer2;
                }

            }
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2 的参数A", ABCDJ2[0].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2 的参数B", ABCDJ2[1].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2的参数C", ABCDJ2[2].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2 的参数D", ABCDJ2[3].ToString("0.000000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2的内部点数量", (innermax2 - 3).ToString()) + "\n";
            text += String.Format("{0,-20}{1,-20}", "拟合平面J2 的外部点数量", outermin2.ToString()) + "\n";

            //MessageBox.Show(originpoints.Count.ToString());
            double[] xyz5 = Segment.GetProject(originpoints[4], ABCDJ1);
            double[] xyz800 = Segment.GetProject(originpoints[799], ABCDJ1);

            text += String.Format("{0,-20}{1,-20}", "P5 点到最佳分割面（J1）的投影坐标xt", xyz5[0].ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P5 点到最佳分割面（J1）的投影坐标yt", xyz5[1].ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P5 点到最佳分割面（J1）的投影坐标zt", xyz5[2].ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P800点到最佳分割面（J1）的投影坐标xt", xyz800[0].ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P800 点到最佳分割面（J1）的投影坐标yt", xyz800[1].ToString("0.000")) + "\n";
            text += String.Format("{0,-20}{1,-20}", "P800点到最佳分割面（J1）的投影坐标zt", xyz800[2].ToString("0.000")) + "\n";

            for(int i = 0; i < originpoints.Count; i++)
            {
                double dist = Segment.GetP2S(originpoints[i], ABCDJ1);
                double dist2 = Segment.GetP2S(originpoints[i], ABCDJ2);
                if (dist < 0.1) {
                    originpoints[i].label = "J1"; }
                else if(dist2<0.1)
                {
                    originpoints[i].label = "J2";
                }
                else
                {
                    originpoints[i].label = "0";
                }
            }


            for (int i = 0; i < originpoints.Count; i++)
            {
                text += String.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", originpoints[i].Name, originpoints[i].X, originpoints[i].Y, originpoints[i].Z, originpoints[i].label) + "\n";
            }
            richTextBox1.AppendText(text);
            report += text;
            tabControl1.SelectedIndex = 1;
            MessageBox.Show("平面分割成功！");
        }

        /// <summary>
        /// 工具栏——平面分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSeg_Click(object sender, EventArgs e)
        {
            tsmSeg_Click(sender, e);
        }

        /// <summary>
        /// 菜单栏——帮助信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.导入数据\n" +
                "2.进行栅格化以及相关计算\n" +
                "3.进行平面分割\n" +
                "4.查看计算结果\n" +
                "5.保存计算报告");
        }

        /// <summary>
        /// 工具栏——帮助信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbHelp_Click(object sender, EventArgs e)
        {
            tsmHelp_Click(sender, e);
        }

        /// <summary>
        /// 菜单栏——查看计算报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmViewReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        /// <summary>
        /// 菜单栏——查看计算结果表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmViewResult_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

        }


    }
}
