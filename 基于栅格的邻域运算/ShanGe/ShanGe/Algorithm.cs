using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanGe
{
    class Algorithm
    {
        public static void LinYuStatistic( DataCenter dc, ref DataGridView dataGridView, ref RichTextBox richTextBox)
        {
            List<List<double>> LY_Data = new List<List<double>>();

            foreach (List<double > sublist in dc.LY_data)
            {
                LY_Data.Add(new List<double >(sublist));
            }

            List<List<double>> test = new List<List<double>>();
            LY_Data = Algorithm.addBian(LY_Data);           

            //邻域方差统计
            double[,] fangcha = new double[LY_Data.Count - 2, LY_Data[0].Count - 2];

            for (int i = 1; i < LY_Data[0].Count - 1; i++)
            {
                for (int j = 1; j < LY_Data.Count - 1; j++)
                {
                    List<double> tem = new List<double>();
                    for (int k = 0; k < 3; k++)
                    {
                        tem.Add(LY_Data[j - 1][i - 1 + k]);
                        tem.Add(LY_Data[j][i - 1 + k]);
                        tem.Add(LY_Data[j + 1][i - 1 + k]);
                    }

                    double f = Algorithm.caculate_FangCha(tem);
                    fangcha[j - 1, i - 1] = f;
                }
            }
            //报告输出
            richTextBox.Text += "栅格邻域运算计算结果\n";
            richTextBox.Text += "*********************************************\n";

            richTextBox.Text += "方差统计结果\n";
            for (int i = 0; i < fangcha.GetLength(0); i++)
            {
                for (int j = 0; j < fangcha.GetLength(1); j++)
                {

                    richTextBox.Text+= Convert.ToString(Math.Round( fangcha[i, j],4))+"\t";
                }
                richTextBox.Text += "\n";

            }

            richTextBox.Text += "*********************************************\n";

            MessageBox.Show("邻域统计计算成功");
        }

        public static void Average_LB( DataCenter dc, ref DataGridView dataGridView, ref RichTextBox richTextBox)
        {
            //首先对于行边界进行扩充
            List<List<double>> LY_Data = new List<List<double>>();

            foreach (List<double> sublist in dc.LY_data)
            {
                LY_Data.Add(new List<double>(sublist));
            }

            LY_Data = Algorithm.addBian(LY_Data);

            //均值滤波
            double[,] average = new double[LY_Data.Count - 2, LY_Data[0].Count - 2];

            for (int i = 1; i < LY_Data[0].Count - 1; i++)
            {
                for (int j = 1; j < LY_Data.Count - 1; j++)
                {
                    List<double> tem = new List<double>();
                    for (int k = 0; k < 3; k++)
                    {
                        tem.Add(LY_Data[j - 1][i - 1 + k]);
                        tem.Add(LY_Data[j][i - 1 + k]);
                        tem.Add(LY_Data[j + 1][i - 1 + k]);
                    }

                    double f = tem.Average();
                    average[j - 1, i - 1] = f;
                }
            }

            richTextBox.Text += "均值滤波结果\n";
            for (int i =0 ; i < average.GetLength(0); i++)
            {
                string[] strs = new string[average.GetLength(1)];
                for (int j = 0; j < average.GetLength(1); j++)
                {

                    strs[j] = string.Format("{0,-5}", Convert.ToString(Math.Round(average[i, j], 4)));
                }

                string str1 = string.Join("\t", strs);
                richTextBox.Text += str1 + "\n";
            }
            richTextBox.Text += "*********************************************\n";

            MessageBox.Show("均值滤波计算成功");

        }

        public static double caculate_FangCha(List<double> LinYu)
        {
            double Fangcha = 0.0;
            double a = LinYu.Average();

            for (int i = 0; i < LinYu.Count; i++)
            {
                Fangcha = Fangcha + (LinYu[i] - a) * (LinYu[i] - a) / LinYu.Count;
            }
            return Fangcha;
        }

        //边界扩充函数封装
        public static List<List<double>> addBian(List<List<double>> list)
        {
            List<List<double>> afterAddBian=new List<List<double>>();

            //行数与列数
            int row = list.Count;
            int col = list[0].Count;

            //上下两边插入一列
            list.Insert(0, list[0]);
            list.Insert(row, list[row]);

            //左右两边插入
            for(int i = 1; i < list.Count-1; i++)
            {
                if (i == 1)
                {
                    list[i].Insert(0, list[i][0]);
                    list[i].Insert(list[i].Count-1, list[i][list[i].Count-1]);
                }
                else
                {
                    list[i].Insert(0, list[i][0]);
                    list[i].Insert(list[i].Count-1, list[i][list[i].Count-1]);
                }

            }
            afterAddBian = list;
            return afterAddBian;
        }

        //将list转化为double[,]
        public static double[,] ListToDouble(List<List<double>> list)
        {
            double[,] a = new double[list.Count, list[0].Count];
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = 0; j < list[0].Count; j++)
                {
                    a[i, j] = list[i][j];
                }
            }
            return a;
        }





    }
}
