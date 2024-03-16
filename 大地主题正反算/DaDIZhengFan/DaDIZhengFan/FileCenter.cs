using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaDIZhengFan
{
    class FileCenter
    {
        /// <summary>
        /// 正算文件读取
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="dataGridView"></param>
        public static void openFile_Zheng(ref DataCenter dc, ref DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                {
                    string[] strs;
                    strs = sr.ReadLine().Split(',');
                    dc.a = Convert.ToDouble(strs[0]);
                    dc.e_dao = Convert.ToDouble(strs[1]);

                    int line = 0;
                    while (!sr.EndOfStream)
                    {
                        strs = sr.ReadLine().Split(',');
                        dataGridView.Rows.Add();
                        dc.start_point.Add(strs[0]);
                        dc.B1.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[1])));
                        dc.L1.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[2])));
                        dc.A12.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[3])));
                        dc.S.Add(Convert.ToDouble(strs[4]));
                        dc.end_point.Add(strs[5]);

                        dataGridView.Rows[line].Cells[0].Value = strs[0];
                        dataGridView.Rows[line].Cells[1].Value = strs[1];
                        dataGridView.Rows[line].Cells[2].Value = strs[2];
                        dataGridView.Rows[line].Cells[3].Value = strs[3];
                        dataGridView.Rows[line].Cells[4].Value = strs[4];
                        dataGridView.Rows[line].Cells[5].Value = strs[5];
                        line++;
                    }

                }
            }

            MessageBox.Show("文件读取成功");

        }

        /// <summary>
        /// 反算文件读取
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="dataGridView"></param>
        public static void openFile_Fan(ref DataCenter dc, ref DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                {
                    string[] strs;
                    strs = sr.ReadLine().Split(',');
                    dc.a = Convert.ToDouble(strs[0]);
                    dc.e_dao = Convert.ToDouble(strs[1]);

                    int line = 0;
                    while (!sr.EndOfStream)
                    {
                        strs = sr.ReadLine().Split(',');
                        dataGridView.Rows.Add();
                        dc.start_point.Add(strs[0]);
                        dc.B1.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[1])));
                        dc.L1.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[2])));
                        dc.B2.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[4])));
                        dc.L2.Add(Algorithm.dmdTohd(Convert.ToDouble(strs[5])));
                        dc.end_point.Add(strs[3]);

                        dataGridView.Rows[line].Cells[0].Value = strs[0];
                        dataGridView.Rows[line].Cells[1].Value = strs[1];
                        dataGridView.Rows[line].Cells[2].Value = strs[2];
                        dataGridView.Rows[line].Cells[5].Value = strs[3];
                        dataGridView.Rows[line].Cells[6].Value = strs[4];
                        dataGridView.Rows[line].Cells[7].Value = strs[5];

                        line++;
                    }

                }
            }

            MessageBox.Show("文件读取成功");
        }
    }
}
