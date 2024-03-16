using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanGe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataCenter dc = new DataCenter();


        private void 文件打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Algorithm.LinYuStatistic(dc, ref dataGridView1, ref richTextBox1);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Algorithm.Average_LB(dc, ref dataGridView1, ref richTextBox1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("注意：每次使用一个功能需要重新打开文件");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FileCenter.saveFile(ref richTextBox1);
        }

        private void 文件打开ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1);
        }

        private void 文件保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCenter.saveFile(ref richTextBox1);
        }
    }
}
