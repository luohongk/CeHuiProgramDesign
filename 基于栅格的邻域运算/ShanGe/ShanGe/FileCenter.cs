using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanGe
{
    class FileCenter
    {
        public static void openFile(ref DataCenter dc, ref DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                {
                    string[] strs;
                    strs = sr.ReadLine().Split(' ');
                    string[] strs1 = strs;
                    //动态控制列
                    for (int i = 0; i < strs.Length; i++)
                    {
                        dataGridView.Columns.Add("column" + (i+1).ToString(), "第" + (i+1).ToString() + "列");
                    }

                    //将第一行添加进去
                    dataGridView.Rows.Add();
                    //初始化
                    dc.LY_data.Add(new List<double>());
                    for (int j = 0; j < strs1.Length; j++)
                    {
                        dataGridView.Rows[0].Cells[j].Value = strs1[j];
                        dc.LY_data[0].Add(Convert.ToDouble(strs1[j]));
                    }

                    int line = 1;
                    while (!sr.EndOfStream)
                    {
                        strs = sr.ReadLine().Split(' ');
                        dataGridView.Rows.Add();
                        dc.LY_data.Add(new List<double>());
                        for (int j = 0; j < strs.Length; j++)
                        {
                            dataGridView.Rows[line].Cells[j].Value = strs[j];
                            dc.LY_data[line].Add(Convert.ToDouble(dataGridView.Rows[line].Cells[j].Value));
                        }
                        line++;
                    }
                }
            }


        }

        //文件保存
        public static void saveFile(ref RichTextBox richTextBox)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "文本文件(*.txt)|*.txt";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SaveFile(sd.FileName, RichTextBoxStreamType.PlainText);
                richTextBox.SaveFile(sd.FileName, RichTextBoxStreamType.PlainText);
                MessageBox.Show("保存成功！");
            }

        }
    }
}
