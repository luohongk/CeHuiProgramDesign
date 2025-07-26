using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _11Curve_Fitting
{
    public partial class Form1 : Form
    {
        DataCenter data = new DataCenter();
        Algo go = new Algo();
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开原始数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileCenter.ReadFile(openFileDialog1.FileName, data);
                Report.Open_Show(data, dataGridView1);
                toolStripStatusLabel1.Text = "文件打开成功！";                
            }
        }
        //打开
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            打开原始数据ToolStripMenuItem_Click(sender, e);
        }

        private void 报告txtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileCenter.Save_Text(saveFileDialog1.FileName, richTextBox1.Text);
                toolStripStatusLabel1.Text = "报告保存成功！";
            }
        }
        //保存报告
        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            报告txtToolStripMenuItem_Click(sender, e);
        }

        private void 图形dxfToolStripMenuItem_Click(object sender, EventArgs e)//没有实现
        {
            saveFileDialog1.Filter = "图形文件|*.dxf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "图形保存成功！";
            }
        }
        //保存图形
        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            图形dxfToolStripMenuItem_Click(sender, e);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "是否闭合拟合：是";
            bool is_close = true;

            go.Add_Points_1(data.points);
            go.Cal_Curve(data.points);

            double interval = 0.1;//间隔 interval
            Report.Cal_Chart(data.points, chart1, interval, is_close);
            tabControl1.SelectedTab = tabPage3;

            richTextBox1.Text = Report.Cal_Text(data.points, is_close);

            toolStripStatusLabel1.Text = "闭合拟合计算完毕！";
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            闭合拟合ToolStripMenuItem_Click(sender, e);
        }

        private void 不闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "是否闭合拟合：否";
            bool is_close = false;

            go.Add_Points_2(data.points);
            go.Cal_Curve(data.points);

            double interval = 0.1;//间隔 interval
            Report.Cal_Chart(data.points, chart1, interval, is_close);
            tabControl1.SelectedTab = tabPage3;

            richTextBox1.Text = Report.Cal_Text(data.points, is_close);

            toolStripStatusLabel1.Text = "不闭合拟合计算完毕！";
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            不闭合拟合ToolStripMenuItem_Click(sender, e);
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("使用步骤：\r\n打开——计算——保存");
        }
    }
}
