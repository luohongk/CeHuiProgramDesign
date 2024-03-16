using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class FileCenter
    {
        #region 初始化头部
        public void initHeader(ref DataGridView dataGridView1)
        {
            dataGridView1.Columns[0].HeaderText = "起始点";
            dataGridView1.Columns[1].HeaderText = "B1";
            dataGridView1.Columns[2].HeaderText = "L1";
            dataGridView1.Columns[3].HeaderText = "A1";
            dataGridView1.Columns[4].HeaderText = "S";
            dataGridView1.Columns[5].HeaderText = "终点";
            dataGridView1.Columns[6].HeaderText = "B2";
            dataGridView1.Columns[7].HeaderText = "L2";
            dataGridView1.Columns[8].HeaderText = "A2";
            dataGridView1.AllowUserToAddRows = true;
        }
        #endregion

        #region 正算数据的打开
        public static void openPositiveData(ref DataGridView dataGridView1, ref FileCenter fc,ref DataCenter dc,ref ToolStripTextBox toolStripTextBox1, ref ToolStripTextBox toolStripTextBox2)
        {
            try
            {
                dataGridView1.Rows.Clear();
                //正算输入头部标签
                fc.initHeader(ref dataGridView1);
                //实例化文件打开对象
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                //文件打开
                openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                //openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                //选中文件
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default))
                    {
                        //储存扁率
                        string[] str = sr.ReadLine().Split(',');
                        
                        dc.b = Convert.ToDouble(str[0]);
                        dc.f = Convert.ToDouble(str[1]);
                        toolStripTextBox1.Text = str[0];
                        toolStripTextBox2.Text = str[1];

                        //如果还没有到末尾就增加一行
                        //储存具体数据
                        int line = 0;
                        while (!sr.EndOfStream)
                        {
                            dataGridView1.Rows.Add();
                            str = sr.ReadLine().Split(',');
                            for (int j = 0; j < str.Length; j++)
                            {
                                dataGridView1.Rows[line].Cells[j].Value = str[j];
                            }
                            line++;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 反算数据打开
        public static void invSolutionOpen(ref DataGridView dataGridView1, ref FileCenter fc, ref DataCenter dc, ref ToolStripTextBox toolStripTextBox1, ref ToolStripTextBox toolStripTextBox2)
        {
            try
            {
                dataGridView1.Rows.Clear();
                fc.initHeader(ref dataGridView1);
                //实例化文件打开对象
                OpenFileDialog op = new OpenFileDialog();
                //文件对话框以及文件过滤
                op.Filter = "文本文件(*.txt)|*.txt";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                    {
                        string[] str = sr.ReadLine().Split(',');
                        //存储长半轴和扁率
                        dc.b = Convert.ToDouble(str[0]);
                        dc.f = Convert.ToDouble(str[1]);
                        toolStripTextBox1.Text = str[0];
                        toolStripTextBox2.Text = str[1];

                        //读取
                        //记录行数
                        int line = 0;
                        while (!sr.EndOfStream)
                        {
                            //表格中增加一行
                            dataGridView1.Rows.Add();
                            str = sr.ReadLine().Split(',');
                            dataGridView1.Rows[line].Cells[0].Value = str[0];
                            dataGridView1.Rows[line].Cells[1].Value = str[1];
                            dataGridView1.Rows[line].Cells[2].Value = str[2];
                            dataGridView1.Rows[line].Cells[5].Value = str[3];
                            dataGridView1.Rows[line].Cells[6].Value = str[4];
                            dataGridView1.Rows[line].Cells[7].Value = str[5];
                            line++;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
