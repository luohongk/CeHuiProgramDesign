using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class 大地主题结算 : Form
    {
        public 大地主题结算()
        {
            InitializeComponent();
        }
        FileCenter fc = new FileCenter();
        DataCenter dc = new DataCenter();
        Algorithm al = new Algorithm();
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {

        }


        private void toolStripButton1_Click_3(object sender, EventArgs e)
        {   
            
            FileCenter.openPositiveData(ref dataGridView1, ref fc, ref dc,ref toolStripTextBox1,ref toolStripTextBox2);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Algorithm.invSolution(ref dc, ref fc, ref dataGridView1);
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileCenter.invSolutionOpen(ref dataGridView1, ref fc, ref dc,ref toolStripTextBox1, ref toolStripTextBox2);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Algorithm.positiveDataSolution(ref dc, ref fc, ref dataGridView1);
        }


    }
}
