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

namespace SpatialDataAnalysis
{
    public partial class Form1 : Form
    {
        string text = null;
        DataCenter data = new DataCenter();
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                text += FileCenter.ReadFile(openFileDialog1.FileName, data, dataGridView1);
                
                toolStripStatusLabel1.Text = "程序运行状态：文件读取成功！";
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            text += Algorithm.CalEllipse(data, chart1);

            tabControl1.SelectedTab = tabPage3;
            richTextBox1.Text = text;
            toolStripStatusLabel1.Text = "程序运行状态：标准差椭圆计算完毕！";
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            text += Algorithm.CalWeightMatrix(data);

            tabControl1.SelectedTab = tabPage2;
            richTextBox1.Text = text;
            toolStripStatusLabel1.Text = "程序运行状态：空间权重矩阵计算完毕！";
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            text += Algorithm.CalMolanIndex(data);

            richTextBox1.Text = text;
            toolStripStatusLabel1.Text = "程序运行状态：莫兰指数计算完毕！";
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                writer.Write(text);
                writer.Close();

                toolStripStatusLabel1.Text = "程序运行状态：文件保存成功！";
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel1_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel5_Click(sender, e);
        }

        private void 标准差椭圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel2_Click(sender, e);
        }

        private void 空间权重矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel3_Click(sender, e);
        }

        private void 莫兰指数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripLabel4_Click(sender, e);
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text = "1.点击打开，选择正式数据.txt\r\n2.点击计算标准差椭圆\r\n" +
                "3.点击计算空间权重矩阵\r\n4.点击计算莫兰指数\r\n5.点击保存\r\n";
            MessageBox.Show(Text);
            
        }
    }
}
