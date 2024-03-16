using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DianYunFenGe
{
    class FileCenter
    {
        public static void openFile(ref DataCenter dc,ref DataGridView dataGridView,ref RichTextBox richTextBox)
        {
            dataGridView.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using(StreamReader sr=new StreamReader(op.FileName, Encoding.Default))
                {
                    string[] strs;

                    //读取总点数
                    strs = sr.ReadLine().Split(',');
                    dc.allPointNum =Convert.ToDouble(strs[0]);
                    int line=0;
                    //读取每一个点的坐标
                    while (!sr.EndOfStream)
                    {
                        strs = sr.ReadLine().Split(',');
                        //增加一行表格
                        dataGridView.Rows.Add();
                        Point point = new Point();
                        point.pointName = strs[0];
                        point.X =Convert.ToDouble( strs[1]);
                        point.Y = Convert.ToDouble(strs[2]);
                        point.Z = Convert.ToDouble(strs[3]);
                        dc.points.Add(point);

                        dc.points_X.Add(Convert.ToDouble(strs[1]));
                        dc.points_Y.Add(Convert.ToDouble(strs[2]));
                        dc.points_Z.Add(Convert.ToDouble(strs[3]));

                        dataGridView.Rows[line].Cells[0].Value= strs[0];
                        dataGridView.Rows[line].Cells[1].Value = strs[1];
                        dataGridView.Rows[line].Cells[2].Value = strs[2];
                        dataGridView.Rows[line].Cells[3].Value = strs[3];

                        line++;
                    }
                }
            }
        }

        public static void saveFile(ref DataCenter dc, ref RichTextBox richTextBox)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "result.txt";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SaveFile(sd.FileName, RichTextBoxStreamType.PlainText);
            }
        }
    }
}
