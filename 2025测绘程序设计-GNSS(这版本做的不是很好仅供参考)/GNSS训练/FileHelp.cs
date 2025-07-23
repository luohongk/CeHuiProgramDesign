using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS训练
{
    internal class FileHelp
    {
        public static void initable(DataTable table)
        {
            table.Clear();
            table.Columns.Add("id", typeof(string));
            table.Columns.Add("Satname", typeof(string));
            table.Columns.Add("P1", typeof(double));
            table.Columns.Add("P2", typeof(double));
            table.Columns.Add("P3", typeof(double));
            table.Columns.Add("Phi1", typeof(double));
            table.Columns.Add("Phi2", typeof(double));
            table.Columns.Add("Phi3", typeof(double));
        }
        public static void Open(MyAlog data, DataTable table)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "读取文本";
            open.Filter = "文本文件|*.txt";
            if (open.ShowDialog() == DialogResult.OK)
            {
                using (var sr = new StreamReader(open.FileName))
                {
                    sr.ReadLine();
                    var bvr = sr.ReadLine().Split(',').Select(t => double.Parse(t)).ToList();
                    MyAlog.f1 = bvr[0];
                    MyAlog.f2 = bvr[1];
                    MyAlog.f3 = bvr[2];
                    MyAlog.c = bvr[3];
                    sr.ReadLine();
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var bvr1 = sr.ReadLine().Split(',');
                        MyPoint point = new MyPoint(bvr1[0], bvr1[1], double.Parse(bvr1[2]),
                            double.Parse(bvr1[3]), double.Parse(bvr1[4]), double.Parse(bvr1[5])
                            , double.Parse(bvr1[6]), double.Parse(bvr1[7]));
                        data.points.Add(point);
                    }


                }
                foreach (var ite in data.points)
                {
                    DataRow rows = table.NewRow();
                    rows["id"] = ite.Time;
                    rows["Satname"] = ite.SatName;
                    rows["P1"] = ite.P1;
                    rows["P2"] = ite.P2;
                    rows["P3"] = ite.P3;
                    rows["Phi1"] = ite.phi1;
                    rows["Phi2"] = ite.phi2;
                    rows["Phi3"] = ite.phi3;
                    table.Rows.Add(rows);
                }

            }
        }
        public static void Save(string data)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存文件";
            save.Filter = "文本文件|*.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                using (var sr = new StreamWriter(save.FileName))
                {
                    sr.WriteLine(data);
                }
            }
        }
        public static void Help()
        {
            MessageBox.Show("欢迎使用本程序!\n1.首先打开读取文件按钮\n2.再点击计算按钮\n3.最后点击保存按钮");
        }
    }
}
