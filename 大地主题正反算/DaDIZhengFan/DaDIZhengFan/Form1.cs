using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaDIZhengFan
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

        private void 文件保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("现在的时间为"+DateTime.Now.ToString());
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本软件的开发人员为罗宏昆\n" + "使用方法为依次点击工具条即可");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileCenter.openFile_Zheng(ref dc, ref dataGridView1);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FileCenter.openFile_Fan(ref dc, ref dataGridView1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Algorithm.caculate_Zheng(ref dc, ref richTextBox1,ref dataGridView1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Algorithm.caculate_Fan(ref dc, ref richTextBox1, ref dataGridView1);
        }
    }
}
