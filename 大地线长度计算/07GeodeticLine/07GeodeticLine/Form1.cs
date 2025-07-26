using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace _07GeodeticLine
{
    public partial class Form1 : Form
    {
        DataCenter data = new DataCenter();
        public static string ReportText = null;
        FileCenter fileCenter = new FileCenter();
        Algo go = new Algo();
        public Form1()
        {
            InitializeComponent();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {                
                fileCenter.ReadFile(openFileDialog1.FileName, data);

                dataGridView1[1, 0].Value = fileCenter.buf1[0];
                dataGridView1[2, 0].Value = fileCenter.buf1[1];
                dataGridView1[1, 1].Value = fileCenter.buf2[0];
                dataGridView1[2, 1].Value = fileCenter.buf2[1];

                toolStripStatusLabel1.Text = "文件导入成功！";
            }
        }

        private void 报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportText = null;
            ReportText += "-------------计算报告-------------\r\n\n";
            if (克拉索夫斯基椭球ToolStripMenuItem.Checked == true) 
                ReportText += "本次计算采用克拉索夫斯基椭球\r\n\n";
            else if(iUGG1975椭球ToolStripMenuItem.Checked == true)
                ReportText += "本次计算采用 IUGG 1957 椭球\r\n\n";
            else
                ReportText += "本次计算采用 CGCS 2000 椭球\r\n\n";
            ReportText += $"大地线起点 P1 的大地坐标({fileCenter.buf1[0]},{fileCenter.buf1[1]})\r\n";
            ReportText += $"大地线起点 P2 的大地坐标({fileCenter.buf2[0]},{fileCenter.buf2[1]})\r\n\n";
            ReportText += $"计算得到大地线长度 S ：{go.S:f4}(m)\r\n";

            Report report = new Report();            
            report.ShowDialog();
            toolStripStatusLabel1.Text = "报告生成完毕！";
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            go.Cal_Help(data);
            go.Cal_A1(data);
            go.Cal_S(data);

            dataGridView2[0, 0].Value = go.S.ToString("f4");
            toolStripStatusLabel1.Text = "计算完毕！";
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件|*.txt";
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                writer.Write(ReportText);
                writer.Close();
                toolStripStatusLabel1.Text = "文件保存成功！";
            }
        }
        /// <summary>
        /// 双击 Form1 表头进行初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 2;
            dataGridView2.RowCount = 1;
            dataGridView1[0, 0].Value = "P1";
            dataGridView1[0, 1].Value = "P2";

            克拉索夫斯基椭球ToolStripMenuItem.Checked = true;
            iUGG1975椭球ToolStripMenuItem.Checked = false;
            cGCS2000椭球ToolStripMenuItem.Checked = false;
            toolStripStatusLabel2.Text = "克拉索夫斯基椭球";

            data.e2 = 0.00669342162297;
            data.e12 = 0.00673852541468;
            data.b = 6356863.0187730473;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void 克拉索夫斯基椭球ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            克拉索夫斯基椭球ToolStripMenuItem.Checked = true;
            iUGG1975椭球ToolStripMenuItem.Checked = false;
            cGCS2000椭球ToolStripMenuItem.Checked = false;
            toolStripStatusLabel2.Text = "克拉索夫斯基椭球";

            data.e2 = 0.00669342162297;
            data.e12 = 0.00673852541468;
            data.b = 6356863.0187730473;
        }

        private void iUGG1975椭球ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            克拉索夫斯基椭球ToolStripMenuItem.Checked = false;
            iUGG1975椭球ToolStripMenuItem.Checked = true;
            cGCS2000椭球ToolStripMenuItem.Checked = false;
            toolStripStatusLabel2.Text = "IUGG 1975 椭球";

            data.e2 = 0.00669438499959;
            data.e12 = 0.00673950181947;
            data.b = 6356755.2881575287;
        }

        private void cGCS2000椭球ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            克拉索夫斯基椭球ToolStripMenuItem.Checked = false;
            iUGG1975椭球ToolStripMenuItem.Checked = false;
            cGCS2000椭球ToolStripMenuItem.Checked = true;
            toolStripStatusLabel2.Text = "CGCS 2000 椭球";

            data.e2 = 0.0066943800390;
            data.e12 = 0.00673949677548;
            data.b = 6356752.3141;
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.RowCount = 2;
            dataGridView2.RowCount = 1;
            dataGridView1[0, 0].Value = "P1";
            dataGridView1[0, 1].Value = "P2";
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}
