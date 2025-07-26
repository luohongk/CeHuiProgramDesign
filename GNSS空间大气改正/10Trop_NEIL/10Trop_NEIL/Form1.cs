using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _10Trop_NEIL
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
                FileCenter.Read_File(openFileDialog1.FileName, data);
                toolStripStatusLabel1.Text = "文件打开成功！";
            }
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            go.Cal_mwE(data);
            go.Cal_mdE(data);

            string text = Report.Cal_Show(data);
            richTextBox1.Text = text;

            toolStripStatusLabel1.Text = "计算完毕！";
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件|*.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                toolStripStatusLabel1.Text = "保存成功！";
            }            
        }
    }
}
