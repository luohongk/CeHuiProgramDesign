using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09Section_Cal
{
    public partial class Form1 : Form
    {
        DataCenter data = new DataCenter();
        
        Algo go = new Algo();
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileCenter.ReadFile(openFileDialog1.FileName, data);                
                Report.OpenShow(data, dataGridView1);
                toolStripStatusLabel1.Text = "文件打开成功！";
                toolStripStatusLabel2.Text = $"基准高程：{data.H0.ToString("f3")}(m)";
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileCenter.WriteFile(saveFileDialog1.FileName, richTextBox1.Text);
                toolStripStatusLabel1.Text = "结果保存成功！";
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 纵断面ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            go.Cal_Vertical(data);
            tabControl1.SelectedTab = tabPage2;//使 tabPage2 直接加载出来，不需要点击
            Report.Cal_Ver_Show(go, richTextBox1);
            
            toolStripStatusLabel1.Text = "纵断面计算完毕！";
        }

        private void 横断面1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            go.Cal_Horizontal1(data);
            tabControl1.SelectedTab = tabPage2;
            Report.Cal_Hor1_Show(go, richTextBox1);
            toolStripStatusLabel1.Text = "横断面 1 计算完毕！";
        }

        private void 横断面2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            go.Cal_Horizontal2(data);
            tabControl1.SelectedTab = tabPage2;
            Report.Cal_Hor2_Show(go, richTextBox1);
            toolStripStatusLabel1.Text = "横断面 2 计算完毕！";
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("操作步骤：\r\n1.文件 - 打开\r\n2.计算\r\n3.文件 - 保存\r");
        }
    }
}
