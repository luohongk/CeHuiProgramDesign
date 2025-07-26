using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 空间后方交汇模拟7._22
{
    internal class FileHelp
    {
        public static void initable(DataTable table)
        {
            table.Clear();
            table.Columns.Add("id",typeof(string));
            table.Columns.Add("x", typeof(double));
            table.Columns.Add("y",typeof(double));
            table.Columns.Add("X",typeof(double));
            table.Columns.Add("Y",typeof(double));
            table.Columns.Add("Z",typeof(double));
        }
        public static void Open(MyAlog data,DataTable table)
        {
            OpenFileDialog open=new OpenFileDialog();
            open.Title = "读取文件";
            open.Filter = "文本文科|*.txt";
            if (open.ShowDialog() == DialogResult.OK)
            {
                using(var sr=new StreamReader(open.FileName))
                {
                    sr.ReadLine();
                    var bvr1 = sr.ReadLine().Split(',').Select(t=>double.Parse(t)).ToList();
                    data.fk=bvr1[0];
                    data.x0= bvr1[1];
                    data.y0= bvr1[2];
                    data.m0= bvr1[3];
                    sr.ReadLine();
                    sr.ReadLine();
                    while(!sr.EndOfStream)
                    {
                        var bvr2 = sr.ReadLine().Split(',');
                        MyData point=new MyData(bvr2[0],double.Parse(bvr2[1]),
                            double.Parse(bvr2[2]), double.Parse(bvr2[3]), double.Parse(bvr2[4])
                            ,double.Parse(bvr2[5]));
                        data.Points.Add(point);
                    }
                }
            }
            foreach(var ite in data.Points)
            {
                DataRow rows=table.NewRow();
                rows["id"] = ite.id;
                rows["x"] = ite.x;
                rows["y"]=ite.y;
                rows["X"] = ite.X;
                rows["Y"]=ite.Y;
                rows["Z"] = ite.Z;
                table.Rows.Add(rows);
            }
        }
        public static void Save(string data)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存文件";
            save.Filter = "文本文件|*.txt";
            if(save.ShowDialog() == DialogResult.OK)
            {
                using(var sr=new StreamWriter(save.FileName))
                {
                    sr.Write(data);
                }
            }
        }
        public static void Help()
        {
            MessageBox.Show("欢迎使用本程序\n1.首先打开文件\n2.然后点击计算\n3.最后点击保存");
        }
    }
}
