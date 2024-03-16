using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QianFangJiaoHui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.Text = "欢迎使用本软件";
            textBox1.Text = DateTime.Now.ToString();
        }

        DataCenter dc = new DataCenter();

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileCenter.openFile(ref dc, ref dataGridView1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Algorithm.caculate_XYZ(ref dc, ref dataGridView1,ref richTextBox2);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.InitializeComponent();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FileCenter.saveFile(ref dc, ref dataGridView1, ref richTextBox2);
        }
    }
}
