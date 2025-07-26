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

namespace 省赛模拟2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MyAlog data = new MyAlog();
        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "文本文件 (*.txt)|*.txt";
            if (open.ShowDialog()==DialogResult.OK)
            {
                using (var sr = new StreamReader(open.FileName))
                {
                    sr.ReadLine();
                    var bvr1 = sr.ReadLine().Split(',');
                    data.L = bvr1[0];
                    data.B = bvr1[1];
                    data.scal1 = bvr1[2];
                    sr.ReadLine();
                    var bvr2 = sr.ReadLine().Split(',');
                    data.OldTf = bvr2[0];
                    data.scal2 = bvr2[1];
                    sr.ReadLine();
                    var bvr3 = sr.ReadLine().Split(',');
                    data.NewTf = bvr3[0];
                    data.scal3 = bvr3[1];
                    StringBuilder read = new StringBuilder();
                    read.AppendLine($"-------------第一个经纬度数据-------------");
                    read.AppendLine($"经度:{bvr1[0]}<--->纬度:{bvr1[1]}<--->比例尺:{bvr1[2]}");
                    read.AppendLine($"-------------第二个传统图数据-------------");
                    read.AppendLine($"传统图幅:{bvr2[0]}<--->比例尺:{bvr2[1]}");
                    read.AppendLine($"-------------第三个新图幅数据-------------");
                    read.AppendLine($"新图幅:{bvr3[0]}<--->比例尺{bvr3[1]}");
                    richTextBox1.Text = read.ToString();
                    MessageBox.Show("读取文件成功");
                }
            }
        }

        private void 打印PToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("计算成功");
            richTextBox2.Text = data.BG();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "文本文件 (*.txt)|*.txt";
            if(save.ShowDialog()==DialogResult.OK)
            {
                using(var sr=new StreamWriter(save.FileName))
                {
                    sr.Write(richTextBox2.Text);
                }
            }
            MessageBox.Show("保存文件成功");
        }
    }
}
