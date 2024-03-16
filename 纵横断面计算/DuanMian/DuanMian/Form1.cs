using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuanMian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataCenter dc = new DataCenter();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 文件打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1);
        }

        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algorithm.caculateZong(ref dc,ref richTextBox1);
        }

        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Algorithm.caculateHeng(ref dc, ref richTextBox1);
        }
    }
}
