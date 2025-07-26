using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation1_PointCloudSegmentation
{
    public partial class Form1 : Form
    {
        string text = null;
        DataCenter data = new DataCenter();
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                text += FileCenter.ReadFile(openFileDialog1.FileName, data, dataGridView1);
                richTextBox1.Text = text;
            }
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text += Algorithm.ToGrid(data);
            text += Algorithm.Segmentation(data);
            richTextBox1.Text = text;

            tabControl1.SelectedTab = tabPage2;
        }
    }
}
