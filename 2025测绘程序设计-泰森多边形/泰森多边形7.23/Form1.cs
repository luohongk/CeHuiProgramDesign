using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 泰森多边形7._23
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FileHelp.initable(table);
        }
        MyAlog data=new MyAlog();
        DataTable table=new DataTable();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileHelp.Open(data,table);
            dataGridView1.DataSource = table;
            toolStripStatusLabel2.Text = "读取成功!";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = data.PrintBG();
            toolStripStatusLabel6.Text = "计算成功!";
            tabControl1.SelectedIndex = 1;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileHelp.Save(richTextBox1.Text);
            toolStripStatusLabel4.Text = "保存成功!";
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FileHelp.Draw_TB_WithVoronoi(data, chart1);
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FileHelp.Help();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton6_Click(sender, e);
        }
    }
}
