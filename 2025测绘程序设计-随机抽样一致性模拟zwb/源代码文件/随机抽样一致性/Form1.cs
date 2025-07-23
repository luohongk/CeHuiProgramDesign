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

namespace 空间_后方交汇
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initable();
        }
        MyAlog data = new MyAlog();
        DataTable table = new DataTable();
        void initable()
        {
            table.Clear();
            table.Columns.Add("id", typeof(string));
            table.Columns.Add("x", typeof(double));
            table.Columns.Add("y", typeof(double));
            table.Columns.Add("z", typeof(double));
        }
        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "打开文件";
            open.Filter = "文本文件|*.txt";
            if(open.ShowDialog()==DialogResult.OK)
            {
                using (var sr = new StreamReader(open.FileName))
                {
                    data.PointNumber = int.Parse(sr.ReadLine());
                    while(!sr.EndOfStream)
                    {
                        var bvr = sr.ReadLine().Split(',');
                        MyPoint point = new MyPoint(bvr[0], double.Parse(bvr[1]), double.Parse(bvr[2]), double.Parse(bvr[3]));
                        MyAlog.points.Add(point);
                    }
                }
                foreach(var ite in MyAlog.points)
                {
                    DataRow rows = table.NewRow();
                    rows["id"] = ite.id;
                    rows["x"] = ite.x;
                    rows["y"] = ite.y;
                    rows["z"] = ite.z;
                    table.Rows.Add(rows);
                }
                dataGridView1.DataSource = table;
            }
            toolStripStatusLabel2.Text = "读取成功";


        }

        private void 帮助LToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本程序操作指南\n1.先打开文件\n2.在点击解算按钮 \n3.再点击保存按钮");
        }

        private void 未读取_Click(object sender, EventArgs e)
        {

        }

        private void 打印PToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = data.PringBG();
            toolStripStatusLabel4.Text = "解算成功";


        }

        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {

        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            draw();
        }
        void draw()
        {

        }
        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存计算结果";
            save.Title = "文本文件|*.txt";
            if(save.ShowDialog()==DialogResult.OK)
            {
                using (var sr = new StreamWriter(save.FileName))
                {
                    sr.Write(richTextBox1.Text);
                }
            }
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "打开文件";
            open.Filter = "文本文件|*.txt";
            if (open.ShowDialog() == DialogResult.OK)
            {
                using (var sr = new StreamReader(open.FileName))
                {
                    data.PointNumber = int.Parse(sr.ReadLine());
                    while (!sr.EndOfStream)
                    {
                        var bvr = sr.ReadLine().Split(',');
                        MyPoint point = new MyPoint(bvr[0], double.Parse(bvr[1]), double.Parse(bvr[2]), double.Parse(bvr[3]));
                        MyAlog.points.Add(point);
                    }
                }
                foreach (var ite in MyAlog.points)
                {
                    DataRow rows = table.NewRow();
                    rows["id"] = ite.id;
                    rows["x"] = ite.x;
                    rows["y"] = ite.y;
                    rows["z"] = ite.z;
                    table.Rows.Add(rows);
                }
                dataGridView1.DataSource = table;
            }
            toolStripStatusLabel2.Text = "读取成功";
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存计算结果";
            save.Title = "文本文件|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                using (var sr = new StreamWriter(save.FileName))
                {
                    sr.Write(richTextBox1.Text);
                }
            }
        }
    }
}
