using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DianYunFenGe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataCenter dc = new DataCenter();

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("计算流程：先文件打开，后进行计算，然后进行文件保存");
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileCenter.saveFile(ref dc,ref richTextBox1);
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 文件保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCenter.saveFile(ref dc,ref richTextBox1);
        }

        private void 文件打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1, ref richTextBox1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1, ref richTextBox1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Algorithm.caculate_main(ref dc, ref richTextBox1);
        }
    }
}
