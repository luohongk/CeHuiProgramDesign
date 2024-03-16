using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS_SPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataCenter datacenter=new DataCenter();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dataGridView1,ref textBox2, ref textBox3, ref textBox4,ref datacenter);
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("开发人员：罗宏昆；使用方法：先打开文件，后进行计算");
        }

        private void 文件打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dataGridView1, ref textBox2, ref textBox3, ref textBox4, ref datacenter);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Algorithm.caaulate(ref datacenter, ref textBox2, ref textBox3, ref textBox4);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FileCenter.outputRichText(ref richTextBox1, datacenter);
        }
    }
}
