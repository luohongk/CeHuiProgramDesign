using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuanMian
{
    class FileCenter
    {
        public static void openFile(ref DataCenter dc,ref  DataGridView datagridview)
        {
            datagridview.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName, Encoding.Default))
                {                   
                    string[] strs;
                    strs = sr.ReadLine().Split(',');
                    dc.H0 = Convert.ToDouble(strs[1]);
                    strs = sr.ReadLine().Split(',');
                    strs = sr.ReadLine().Split(',');

                    int line = 0;
                    while (!sr.EndOfStream)
                    {                   
                        strs = sr.ReadLine().Split(',');
                        dc.pointName.Add(strs[0]);
                        dc.x.Add(Convert.ToDouble(strs[1]));
                        dc.y.Add(Convert.ToDouble(strs[2]));
                        dc.z.Add(Convert.ToDouble(strs[3]));

                        datagridview.Rows.Add();
                        datagridview.Rows[line].Cells[0].Value=strs[0];
                        datagridview.Rows[line].Cells[1].Value = strs[1];
                        datagridview.Rows[line].Cells[2].Value = strs[2];
                        datagridview.Rows[line].Cells[3].Value = strs[3];
                        line++;
                    }

                }
            }
        }
    }
}
