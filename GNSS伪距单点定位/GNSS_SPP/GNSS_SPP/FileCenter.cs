using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS_SPP
{
    class FileCenter
    {
        public static void openFile(ref DataGridView datagridview, ref TextBox texBox1, ref TextBox texBox2, ref TextBox texBox3, ref DataCenter dc)
        {
            datagridview.Rows.Clear();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "文本文件(*.txt)|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sc = new StreamReader(op.FileName, Encoding.Default))
                {
                    //读取粗略坐标
                    string[] strs;
                    string str;
                    str = sc.ReadLine();
                    texBox1.Text = str.Substring(18, 13);
                    texBox2.Text = str.Substring(35, 12);
                    texBox3.Text = str.Substring(51, 12);
                    str = sc.ReadLine();
                    int line = 0;
                    //读取信息
                    try
                    {
                        while (!sc.EndOfStream)
                        {
                            str = sc.ReadLine();
                            if(str=="")
                            {
                                break;
                            }
                            if (str[0] == 'S')
                            {
                                int count = 0;
                                Position Pos = new Position();
                                strs = str.Split(',');
                                Pos.SatNum = Convert.ToInt16(strs[0].Substring(strs[0].Length - 2).Trim());
                                Pos.GPS_Time = Convert.ToInt32(strs[1].Substring(strs[1].Length - 6).Trim());
                                do
                                {
                                    Position Pos1 = new Position();
                                    Pos1.SatNum = Pos.SatNum;
                                    Pos1.GPS_Time = Pos.GPS_Time;
                                    strs = sc.ReadLine().Split(',');
                                    Pos1.PRN = strs[0];
                                    Pos1.x = Convert.ToDouble(strs[1]);
                                    Pos1.y = Convert.ToDouble(strs[2]);
                                    Pos1.z = Convert.ToDouble(strs[3]);
                                    Pos1.SatClock = Convert.ToDouble(strs[4]);
                                    Pos1.Elevation = Convert.ToDouble(strs[5]);
                                    Pos1.L = Convert.ToDouble(strs[6]);
                                    Pos1.Delay = Convert.ToDouble(strs[7]);
                                    datagridview.Rows.Add();
                                    datagridview.Rows[line].Cells[0].Value = Convert.ToString(Pos1.GPS_Time);
                                    for (int i = 1; i < 8; i++)
                                    {
                                        datagridview.Rows[line].Cells[i].Value = strs[i - 1];
                                    }
                                    line++;
                                    dc.positions.Add(Pos1);
                                    count++;
                                }
                                while (count < Pos.SatNum);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public static void outputRichText(ref RichTextBox richTextBox1,DataCenter dataCenter)
        {
            richTextBox1.Text = "**************************************************\n\n";
            richTextBox1.Text += "                  GNSS伪距单点定位结果\n\n";
            richTextBox1.Text += "**************************************************\n\n";
            richTextBox1.Text += "----------------------数据展示---------------------\n";
            richTextBox1.Text += "观测历元     " + "平差后的坐标X    "+ "平差后的坐标Y    "+ "平差后的坐标Z    \n";
            for(int i = 0; i < dataCenter.truePositions.Count; i++)
            {
                richTextBox1.Text += string.Format("{0, -8}", Convert.ToString( dataCenter.truePositions[i].LiYuan)+"\t" + Convert.ToString(dataCenter.truePositions[i].x) + "\t" + Convert.ToString(dataCenter.truePositions[i].y) + "\t" + Convert.ToString(dataCenter.truePositions[i].z) +"\n\n");
            }
        }

        //此处可以增加一个文件保存的操作，用savefilediolog
    }
}
