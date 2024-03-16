using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QianFangJiaoHui
{
    class FileCenter
    {
        /// <summary>
        /// 文件打开模块
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="dataGridView"></param>
        public static void openFile(ref DataCenter dc, ref DataGridView dataGridView)
        {
            //清空表格
            dataGridView.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                {
                    string[] strs;

                    int line = 0;
                    while (!sr.EndOfStream)
                    {
                        dataGridView.Rows.Add();
                        strs = sr.ReadLine().Split(',');
                        for (int i = 0; i < strs.Length; i++)
                        {
                            dataGridView.Rows[line].Cells[i].Value = strs[i];
                        }
                        dc.Xs.Add(Convert.ToDouble(strs[0]));
                        dc.Ys.Add(Convert.ToDouble(strs[1]));
                        dc.Zs.Add(Convert.ToDouble(strs[2]));
                        dc.phi.Add(Convert.ToDouble(strs[3]));
                        dc.omega.Add(Convert.ToDouble(strs[4]));
                        dc.k.Add(Convert.ToDouble(strs[5]));
                        dc.x.Add(Convert.ToDouble(strs[6]));
                        dc.y.Add(Convert.ToDouble(strs[7]));
                        dc.f.Add(Convert.ToDouble(strs[8]));

                        
                        line++;
                    }

                    MessageBox.Show("文件导入成功");



                }
            }
        }

        /// <summary>
        /// 文件保存模块
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="dataGridView"></param>
        /// <param name="richTextBox"></param>
        public static void saveFile(ref DataCenter dc, ref DataGridView dataGridView, ref RichTextBox richTextBox)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "result.txt";
            sd.Filter = "文本文件(*.txt)|*.txt";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SaveFile(sd.FileName, RichTextBoxStreamType.PlainText);
            }

        }
    }
}
