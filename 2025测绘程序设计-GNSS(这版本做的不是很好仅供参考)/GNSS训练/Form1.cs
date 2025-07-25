using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS训练
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
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileHelp.Save(richTextBox1.Text);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = data.PrintBG();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FileHelp.Help();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(sender, e);
        }
    }
}
