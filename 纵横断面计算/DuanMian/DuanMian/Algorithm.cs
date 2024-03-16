using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DuanMian
{
    class Algorithm
    {
        public static void caculateZong(ref DataCenter dc,ref RichTextBox richTextBox)
        {
            //纵断面长度计算
            List<double> px = new List<double>();
            List<double> py = new List<double>();
            List<double> ph = new List<double>();
            List<double> licheng = new List<double>();
            List<double> isKeyPoint = new List<double>();
            for (int i = 0; i < dc.pointName.Count; i++)
            {
                if (dc.pointName[i].Contains("K"))
                {
                    px.Add(dc.x[i]);
                    py.Add(dc.y[i]);
                    ph.Add(dc.z[i]);
                }
            }
            licheng.Add(0);
            //计算总里程
            double L = 0;
            for (int i = 0; i < px.Count - 1; i++)
            {
                L = L + Math.Sqrt(Math.Pow(px[i + 1] - px[i], 2) + Math.Pow(py[i + 1] - py[i], 2));
                licheng.Add(L);
            }
            //设置关键点的标签
            for (int i = 0; i < dc.pointName.Count; i++)
            {
                if (dc.pointName[i].Contains("K"))
                {
                    isKeyPoint.Add(1);
                }
                else
                {
                    isKeyPoint.Add(0);
                }
            }

            #region 计算内插值
            //内插高程
            double InsertNum = Math.Floor(L / 10);
            List<double> Insert_X = new List<double>();
            List<double> Insert_Y = new List<double>();
            List<double> Insert_H = new List<double>();
            List<double> Insert_licheng = new List<double>();
            double x = px[0];
            double y = py[0];
            double h = 0.0;

            //关键点的计数
            int m = 0;
            //插入点计数
            int n = 0;

            while (true)
            {

                double delta = 10;
                x = x + delta * Math.Cos(Algorithm.CaculateFangwei(px[m], py[m], px[m + 1], py[m + 1]));
                y = y + delta * Math.Sin(Algorithm.CaculateFangwei(px[m], py[m], px[m + 1], py[m + 1]));
                h = Algorithm.CaculateH(x, y, dc);
                Insert_X.Add(x);
                Insert_Y.Add(y);
                Insert_H.Add(h);
                Insert_licheng.Add(10 * (n + 1));
                n++;
                if (Insert_X.Count == InsertNum)
                {
                    break;
                }
                if ((n + 1) * 10 > licheng[m + 1])
                {
                    m++;
                    double tem = (n + 1) * 10 - licheng[m + 1];
                    x = px[m + 1] + tem * Math.Cos(Algorithm.CaculateFangwei(px[m], py[m], px[m + 1], py[m + 1]));
                    y = py[m + 1] + tem * Math.Sin(Algorithm.CaculateFangwei(px[m], py[m], px[m + 1], py[m + 1]));
                    h = Algorithm.CaculateH(x, y, dc);
                    Insert_X.Add(x);
                    Insert_Y.Add(y);
                    Insert_H.Add(h);
                    Insert_licheng.Add(10 * (n + 1));
                    n++;
                }
            }
            #endregion
            #region 计算纵断面的面积
            //计算纵断面的面积
            int u = 0;
            Insert_licheng.AddRange(licheng);
            Insert_X.AddRange(px);
            Insert_Y.AddRange(py);
            Insert_H.AddRange(ph);
            List<double> lichengSort = new List<double>(Insert_licheng);
            lichengSort.Sort();
            //面积计数
            double Area = 0.0;
            for (int i = 0; i < lichengSort.Count - 1; i++)
            {

                double x1 = Insert_X[Insert_licheng.IndexOf(lichengSort[i])];
                double y1 = Insert_Y[Insert_licheng.IndexOf(lichengSort[i])];
                double h1 = Insert_H[Insert_licheng.IndexOf(lichengSort[i])];
                double x2 = Insert_X[Insert_licheng.IndexOf(lichengSort[i + 1])];
                double y2 = Insert_Y[Insert_licheng.IndexOf(lichengSort[i + 1])];
                double h2 = Insert_H[Insert_licheng.IndexOf(lichengSort[i + 1])];
                Area = Area + Algorithm.CaculateArea(x1, y1, h1, x2, y2, h2, dc.H0);
            }
            #endregion
            #region 生成计算报告
            richTextBox.Text = "纵横断面计算结果\n\n";
            richTextBox.Text = richTextBox.Text + "纵断面计算结果\n****************************************"+"\n";
            richTextBox.Text = richTextBox.Text + "纵断面全长：" + Math.Round(L, 5) + "\n";
            richTextBox.Text = richTextBox.Text + "纵断面面积：" + Math.Round(Area, 5) + "\n";
            richTextBox.Text = richTextBox.Text + "纵断面面积纵断面内差点信息\n";
            richTextBox.Text = richTextBox.Text + "内插点名称\t\t" + "内插点X(m)\t\t" + "内插点Y(m)\t\t" + "内插点H(m)\t\t" + "内插点里程L\n";


            for (int i = 0; i < InsertNum; i++)
            {
                string[] strs = new string[5];
                strs[0] = string.Format("{0,8}", "Cha-" + Convert.ToString(i + 1));
                strs[1] = string.Format("{0,8}", Insert_X[i]);
                strs[2] = string.Format("{0,8}", Insert_Y[i]);
                strs[3] = string.Format("{0,8}", Insert_H[i]);
                strs[4] = string.Format("{0,8}", Insert_licheng[i]);
                string str = string.Join("\t", strs);
                richTextBox.Text = richTextBox.Text + str+"\n";

            }
            MessageBox.Show("计算完毕，报告生成完毕");
            


            #endregion
        }

        public static void caculateHeng(ref DataCenter dc,ref RichTextBox richTextBox)
        {
            List<double> px = new List<double>();
            List<double> py = new List<double>();
            List<double> ph = new List<double>();
            List<double> licheng = new List<double>();
            List<double> isKeyPoint = new List<double>();
            for (int i = 0; i < dc.pointName.Count; i++)
            {
                if (dc.pointName[i].Contains("K"))
                {
                    px.Add(dc.x[i]);
                    py.Add(dc.y[i]);
                    ph.Add(dc.z[i]);
                }
            }

            #region 计算横断面的内插坐标

            //横断面的中心点
            List<double> HengCenter_X = new List<double>();
            List<double> HengCenter_Y = new List<double>();
            List<double> HengFWJ = new List<double>();
            List<double> Heng_Insert_X = new List<double>();
            List<double> Heng_Insert_Y = new List<double>();
            List<double> Heng_Insert_H = new List<double>();


            //保证健壮性
            for (int i = 0; i < px.Count - 1; i++)
            {
                HengCenter_X.Add(px[i] + px[i + 1] / 2);
                HengCenter_Y.Add(py[i] + py[i + 1] / 2);
                HengFWJ.Add(Algorithm.CaculateFangwei(px[i], py[i], px[i + 1], py[i + 1])+Math.PI/2);
            }

            //横断面内插
            double delta = 5;
            for (int i = 0; i < HengCenter_X.Count; i++)
            {
                for(int j = -5; j <= 5; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }
                    double x, y,h;
                    x = HengCenter_X[i] + j * delta * Math.Cos(HengFWJ[i]);
                    y = HengCenter_Y[i] + j * delta * Math.Sin(HengFWJ[i]);
                    h = Algorithm.CaculateH(x, y, dc);

                    Heng_Insert_X.Add(x);
                    Heng_Insert_Y.Add(y);
                    Heng_Insert_H.Add(h);

                }
            }
            #endregion
        }
        //坐标方位角封装的函数
        public static double CaculateFangwei(double x1, double y1, double x2, double y2)
        {
            double alpha = 0.0;
            double delta_x = 0.0, delta_y = 0.0;
            delta_x = x2 - x1;
            delta_y = y2 - y1;
            alpha = Math.Atan(delta_y / delta_x);
            if (delta_x < 0)
            {
                alpha = alpha + Math.PI;
            }
            return alpha;
        }
        //距离计算封装函数
        public static double CaculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
        //高程内插的封装函数
        public static double CaculateH(double x, double y, DataCenter dc)
        {
            double H = 0.0;
            List<double> D = new List<double>();
            List<double> h = new List<double>();
            for (int i = 0; i < dc.pointName.Count; i++)
            {
                double d = Math.Sqrt((x - dc.x[i]) * (x - dc.x[i]) + (y - dc.y[i]) * (y - dc.y[i]));
                D.Add(d);
                h.Add(dc.z[i]);
            }
            List<double> tem = new List<double>(D);
            tem.Sort();
            double a = 0.0;
            double b = 0.0;
            int index;
            for (int i = 0; i < 5; i++)
            {
                index = D.IndexOf(tem[i]);
                a = a + h[index] / D[index];
                b = b + 1 / D[index];
                //for (int j = 0; j < dc.pointName.Count; j++)
                //{
                //    double dis = Math.Sqrt((x - dc.x[j]) * (x - dc.x[j]) + (y - dc.y[j]) * (y - dc.y[j]));
                //    if (dc.pointName[j].Contains("P") && dis == tem[i])
                //    {
                //        a = a + dc.z[j] / dis;
                //        b = b + 1 / dis;
                //    }
                //}
            }
            H = a / b;
            return H;
        }
        //断面面积计算封装函数
        public static double CaculateArea(double x1, double y1, double h1, double x2, double y2, double h2, double H0)
        {
            double area = 0.0;
            double delta_L = Algorithm.CaculateDistance(x1, y1, x2, y2);
            area = (h1 + h2 - 2 * H0) / 2 * delta_L;
            return area;
        }
    }
}
