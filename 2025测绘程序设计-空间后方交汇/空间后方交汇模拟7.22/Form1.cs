using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 空间后方交汇模拟7._22
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
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileHelp.Open(data,table);
            dataGridView1.DataSource = table;
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
    }
}
