using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 省赛模拟2
{
    internal class MyAlog
    {
        //原始数据
        public string B;
        public string L;
        public string scal1;
        public string scal2;
        public string scal3;
        public string NewTf;
        public string OldTf;
        //定义比例尺
        public string D = "1:100000";
        public string E = "1:50000";
        public string F = "1:25000";
        //将经纬差转化为十进制
        public double BC100 = DDMMSSToS("4.0000");
        public double LC100 = DDMMSSToS("6.0000");
        public double BC10 = DDMMSSToS("0.2000");
        public double LC10 = DDMMSSToS("0.3000");
        public double BC5 = DDMMSSToS("0.1000");
        public double LC5 = DDMMSSToS("0.1500");
        public double BC25 = DDMMSSToS("0.0500");
        public double LC25 = DDMMSSToS("0.0730");
        
        public string BG()
        {
            StringBuilder read = new StringBuilder();
            var BS1 = DDMMSSToS(B);
            var LS1 = DDMMSSToS(L);
            //正式数据
            read.AppendLine("----------第一个经纬度数据------------");
            read.AppendLine(($"经度：{B},纬度:{L}"));
            read.AppendLine($"新图幅:{BLToNew(BS1, LS1, scal1)}");
            read.AppendLine($"旧图幅:{BLToOld(BS1, LS1, scal1)}");
            read.AppendLine($"----图廓----");
            read.AppendLine($"{NewTOTK(BLToNew(BS1, LS1, scal1), scal1)}");
            read.AppendLine($"---接图表---");
            var ne1 = NewToBLZX(BLToNew(BS1, LS1, scal1));//这个计算的是左下角图幅经纬度所以后续要加上经纬差的二分之一
            var B12 = ne1.B + BC5 / 2;
            var L12 = ne1.L + LC5 / 2;
            read.AppendLine($"{BLTOTB(B12, L12, scal1)}");
            read.AppendLine("----------第二个旧图幅数据------------");
            read.AppendLine($"旧图幅:{OldTf}");
            read.AppendLine($"新图幅:{OldToNew(OldTf)}");
            var ne3 = NewToBLZX(OldToNew(OldTf));
            var B14 = ne3.B + BC25 / 2;
            var L14 = ne3.L + LC25 / 2;
            read.AppendLine($"经度:{SToDDMMSS(L14)},纬度:{SToDDMMSS(B14)}");
            read.AppendLine($"----图廓----");
            read.AppendLine($"{NewTOTK(OldToNew(OldTf),scal2)}");
            read.AppendLine($"---接图表---");
            read.AppendLine($"{BLTOTB(B14, L14, scal2)}");

            read.AppendLine("----------第三个新图幅数据------------");
            read.AppendLine($"新图幅:{NewTf}");
            var ne2 = NewToBLZX(NewTf);
            var B13 = ne2.B + BC10 / 2;
            var L13 = ne2.L + LC10 / 2;
            read.AppendLine($"旧图幅:{BLToOld(B13, L13, scal3)}");
            read.AppendLine($"经度:{SToDDMMSS(L13)},纬度:{SToDDMMSS(B13)}");
            read.AppendLine($"----图廓----");
            read.AppendLine($"{NewTOTK(NewTf, scal3)}");
            read.AppendLine($"---接图表---");
            read.AppendLine($"{BLTOTB(B13, L13, scal3)}");
            return read.ToString();
        }
        /// <summary>
        /// DDMMSS转十进制（无损失提取）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double DDMMSSToS(string data)
        {
            
            double angel = double.Parse(data);
            angel = angel * 10000.0;
            int myangel = (int)angel;
            int DD = myangel / 10000;
            int MM = (myangel - DD * 10000) / 100;
            double SS = angel - DD * 10000 - MM * 100;

            return DD + MM/60.0+ SS/3600.0;
        }
        /// <summary>
        /// 十进制转DDMMSS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SToDDMMSS(double data)
        {
            var DD = (int)data;
            double MM1 = (data - DD) * 60;
            var MM = (int)MM1;
            double SS = (MM1 - MM) * 60;
            return $"{DD}.{MM:00}{SS:00}";

        }
        /// <summary>
        /// 经纬度转新图幅（记住这个b，l是十进制）
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="scal"></param>
        /// <returns></returns>
        public string BLToNew(double B, double L, string scal)
        {
            string res = "";
            //计算100w的行列号
            char Row100 = (char)((int)(B / 4)+'A');
            int Lie100 = (int)(L / 6) + 31;
            res += $"{Row100}{Lie100}";
            int row=0, lie=0;
            if(scal==D)
            {
                res += "D";
                row =(int) (BC100 / BC10 - (int)((B % BC100) / BC10));
                lie = (int)((L % LC100) / LC10) + 1;
            }
            if (scal == E)
            {
                res += "E";
                row = (int)(BC100 / BC5 - (int)((B % BC100) / BC5));
                lie = (int)((L % LC100) / LC5) + 1;
            }
            if (scal == F)
            {
                res += "F";
                row = (int)(BC100 / BC25 - (int)((B % BC100) / BC25));
                lie = (int)((L % LC100) / LC25) + 1;
            }
            res += $"{row:000}{lie:000}";//这个是补零操作
            return res;
        }
        /// <summary>
        /// 计算左下角经纬度
        /// </summary>
        /// <param name="New1"></param>
        /// <returns></returns>
        public (double B,double L)NewToBLZX(string New1)
        {
            //计算100w的
            int Row100 = (int)(New1[0]-'A'+1);
            int Lie100 = int.Parse(New1.Substring(1, 2));
            //把新图幅小比例尺的行列号提取出来
            int Row = int.Parse(New1.Substring(4, 3));
            int Lie = int.Parse(New1.Substring(7, 3));
            double B=0.00000 ,L=0.0000;//防止精度丢失
            if (New1.Substring(3,1)=="D")
            {
                B = (Row100 - 1) * BC100 + (BC100/BC10-Row)*BC10;
                L = (Lie100 - 31) * LC100 + (Lie - 1) * LC10;
            }
            else if (New1.Substring(3,1) == "E")
            {
                B = (Row100 - 1) * BC100 + (BC100 / BC5 - Row) * BC5;
                L = (Lie100 - 31) * LC100 + (Lie - 1) * LC5;
            }
            else if (New1.Substring(3,1) == "F")
            {
                B = (Row100 - 1) * BC100 + (BC100 / BC25 - Row) * BC25;
                L = (Lie100 - 31) * LC100 + (Lie - 1) * LC25;
            }
            return (B,  L);

        }
        /// <summary>
        /// 经纬度计算旧图幅
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="scal"></param>
        /// <returns></returns>
        public string BLToOld(double B, double L, string scal)
        {
            //这部分先通过计算1:10w的号码，然后再依次计算，

            string res = "";
            // 计算1:100万图幅编号
            int H100 = (int)B / 4 + 1;
            Char H1001 = (char)('A' + H100 - 1);
            int L100 = (int)Math.Floor(L / 6) + 31;
            res += $"{H1001}{L100}";

            // 计算100万的经纬差
            double Lc_100 = L - ((int)L / 6) * 6;
            double Bc_100 = B - ((int)B / 4) * 4;
            //计算10w的经纬差
            double Bc_10 = B - (int)(B / BC10) * BC10;
            double Lc_10 = L - (int)(L / LC10) * LC10;
            double N1 = 0, N2 = 0, N3 = 0;
            int row = 0, col = 0;
            //计算5w的经纬差
            double Bc_5 = Bc_10 - (int)(Bc_10 / BC5) * BC5;
            double Lc_5 = Lc_10 - (int)(Lc_10 / LC5) * LC5;
            

            if (scal == "1:100000") // 1:10万
            {
                row = 12 - (int)(Bc_100 / BC10);
                col = (int)(Lc_100 / LC10);
                N1 = (row - 1) * 12 + col;
                res += $"{N1:000}";
            }
            else if (scal == "1:50000") // 1:5万
            {
                //计算10w的然后依次叠加
                row = 12 - (int)(Bc_100 / BC10);
                col = (int)(Lc_100 / LC10)+1;
                N1 = (row - 1) * 12 + col;
                //计算5w的
                double row1 = 2 - (int)(Bc_10 / BC5);
                double col1 = (int)(Lc_10 / LC5)+1;
                N2 = (row1 - 1) * 2 + col1;
                res += $"{N1:000}{N2:0}";

            }
            else if (scal == "1:25000") // 1:2.5万
            {
                //计算10w的然后依次叠加
                row = 12 - (int)(Bc_100 / BC10);
                col = (int)(Lc_100 / LC10)+1;
                N1 = (row - 1) * 12 + col;
                //计算5w的
                double row1 = 2 - (int)(Bc_10 / BC5);
                double col1 = (int)(Lc_10 / LC5) + 1;
                N2 = (row1 - 1) * 2 + col1;
                //计算2.5w的
                double row2 = 2 - (int)(Bc_5 / BC25);
                double col2 = (int)(Lc_5 / LC5) + 1;
                N3 = (row2 - 1) * 2 + col2;
                res += $"{N1:000}{N2:0}{N3:0}";
            }//记得优化下这部分
            return res;
        }
        /// <summary>
        /// 计算图廓
        /// </summary>
        /// <param name="new1"></param>
        /// <param name="scal"></param>
        /// <returns></returns>
        public string NewTOTK(string new1,string scal)
        {
            //这部分思路是先算出左下角经纬差，然后加减
            var st = NewToBLZX(new1);
            StringBuilder read = new StringBuilder();
            if (scal == "1:100000") // 1:10万
            {
                read.AppendLine($"左下角经度:{SToDDMMSS(st.L)}---左下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"左上角经度:{SToDDMMSS(st.L)}---左上角纬度:{SToDDMMSS(st.B + BC10)}");
                read.AppendLine($"右下角经度:{SToDDMMSS(st.L + LC10)}---右下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"右上角经度:{SToDDMMSS(st.L + LC10)}---右上角纬度:{SToDDMMSS(st.B + BC10)}");
            }
            else if (scal == "1:50000") // 1:5万
            {
                read.AppendLine($"左下角经度:{SToDDMMSS(st.L)}---左下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"左上角经度:{SToDDMMSS(st.L)}---左上角纬度:{SToDDMMSS(st.B + BC5)}");
                read.AppendLine($"右下角经度:{SToDDMMSS(st.L + LC5)}---右下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"右上角经度:{SToDDMMSS(st.L + LC5)}---右上角纬度:{SToDDMMSS(st.B + BC5)}");

            }
            else if (scal == "1:25000") // 1:2.5万
            {
                read.AppendLine($"左下角经度:{SToDDMMSS(st.L)}---左下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"左上角经度:{SToDDMMSS(st.L)}---左上角纬度:{SToDDMMSS(st.B + BC25)}");
                read.AppendLine($"右下角经度:{SToDDMMSS(st.L + LC25)}---右下角纬度:{SToDDMMSS(st.B)}");
                read.AppendLine($"右上角经度:{SToDDMMSS(st.L + LC25)}---右上角纬度:{SToDDMMSS(st.B + BC25)}");
            }
            return read.ToString();
        }
        /// <summary>
        /// 计算接图表
        /// </summary>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="scal"></param>
        /// <returns></returns>
        public string BLTOTB(double B, double L, string scal)
        {
            
            //这部分的思路是先计算出中间的经纬差，然后去左右加减经纬差的到相邻图幅
            StringBuilder read = new StringBuilder();
            
            if (scal == "1:100000") // 1:10万
            {
                read.AppendLine($"左上:{BLToOld(B + BC10, L - LC10, scal)}---上:{BLToOld(B + BC10, L, scal)}---右上:{BLToOld(B + BC10, L + LC10, scal)}");
                read.AppendLine($"左:{BLToOld(B, L - LC10, scal)}---原图表:{BLToOld(B, L, scal)}---右:{BLToOld(B, L + LC10, scal)}");
                read.AppendLine($"左下:{BLToOld(B - BC10, L - LC10, scal)}---下:{BLToOld(B - BC10, L, scal)}---右下:{BLToOld(B - BC10, L + LC10, scal)}");
            }
            else if (scal == "1:50000") // 1:5万
            {
                read.AppendLine($"左上:{BLToOld(B + BC5, L - LC5, scal)}---上:{BLToOld(B + BC5, L, scal)}---右上:{BLToOld(B + BC5, L + LC5, scal)}");
                read.AppendLine($"左:{BLToOld(B, L - LC5, scal)}---原图表:{BLToOld(B, L, scal)}---右:{BLToOld(B, L + LC5, scal)}");
                read.AppendLine($"左下:{BLToOld(B - BC5, L - LC5, scal)}---下:{BLToOld(B - BC5, L, scal)}---右下:{BLToOld(B - BC5, L + LC5, scal)}");

            }
            else if (scal == "1:25000") // 1:2.5万
            {
                read.AppendLine($"左上:{BLToOld(B + BC25, L - LC25, scal)}---上:{BLToOld(B + BC25, L, scal)}---右上:{BLToOld(B + BC25, L + LC25, scal)}");
                read.AppendLine($"左:{BLToOld(B, L - LC25, scal)}---原图表:{BLToOld(B, L, scal)}---右:{BLToOld(B, L + LC5, scal)}");
                read.AppendLine($"左下:{BLToOld(B - BC25, L - LC25, scal)}---下:{BLToOld(B - BC25, L, scal)}---右下:{BLToOld(B - BC25, L + LC25, scal)}");
            }
            return read.ToString();
             
        }
       
        private string OldToNew(string oldMapNumber)
        {
            //48个格，120   1,3，144，12,10行，20,40
            //这部分就是先计算10w的，10w有144个格子先算出来，然后再计算5w的在10w基础上
            string res = $"{oldMapNumber.Substring(0, 3)}";
            if(oldMapNumber.Length==6)//根据长度来判断这个是10w的
            {
                res += "D";
                int H10 = int.Parse(oldMapNumber.Substring(3, 3));
                if(H10%12==0)
                {
                    int h_10 = H10 / 12;
                    res += $"{h_10:000}{012}";
                }
                else
                {
                    int st= (int)H10 /12;
                    int l_10 = H10 - st * 12;
                    int h_10 = st + 1;
                    res += $"{h_10:000}{l_10:000}";

                }
                 
            }
            else if(oldMapNumber.Length==7)//5w的
            {
                res += "E";
                int H10 = int.Parse(oldMapNumber.Substring(3, 3));
                int H5 = int.Parse(oldMapNumber.Substring(6, 1));
                if (H10 % 12 == 0)
                {
                    int h_10 = H10 / 12;
                    int l_10 = 12;
                    int h_5 = h_10 * 2;
                    int l_5 = l_10 * 2;
                    if(H5==1)
                    {
                        res += $"{h_5 - 1:000}{l_5 - 1:000}";
                    }
                    else if(H5==2)
                    {
                        res += $"{h_5 - 1:000}{l_5:000}";
                    }
                    else if(H5==3)
                    {
                        res += $"{h_5:000}{l_5 - 1:000}";
                    }
                    else if(H5==4)
                    {
                        res += $"{h_5:000}{l_5:000}";
                    }
                    
                    
                }
                else
                {
                    int st = (int)H10 / 12;
                    int l_10 = H10 - st * 12;
                    int h_10 = st + 1;
                    
                    int h_5 = h_10 * 2;
                    int l_5 = l_10 * 2;
                    if (H5 == 1)
                    {
                        res += $"{h_5 - 1:000}{l_5 - 1:000}";
                    }
                    else if (H5 == 2)
                    {
                        res += $"{h_5 - 1:000}{l_5:000}";
                    }
                    else if (H5 == 3)
                    {
                        res += $"{h_5:000}{l_5 - 1:000}";
                    }
                    else if (H5 == 4)
                    {
                        res += $"{h_5:000}{l_5:000}";
                    }
                }
            }
            else if (oldMapNumber.Length == 8)//2.5w的这部分可以优化下，就是反复叠加，写个函数，因为时间关系我只根据题目的特例来写
            {
                res += "F";
                int H10 = int.Parse(oldMapNumber.Substring(3, 3));
                int H5 = int.Parse(oldMapNumber.Substring(6, 1));
                int H25 = int.Parse(oldMapNumber.Substring(7, 1));
                if (H10 % 12 == 0)
                {
                    int h_10 = H10 / 12;
                    int l_10 = 12;
                    int h_5 = h_10 * 2;
                    int l_5 = l_10 * 2;
                    if (H5 == 1)
                    {
                        h_5 = h_5 - 1;
                        l_5 = l_5 - 1;
                        int h_25 = h_5 * 2;
                        int l_25 = l_5 * 2;
                        if (H25 == 3)
                        {
                            res += $"{h_25:000}{l_25 - 1:000}";
                        }
                    }
                }
                
            }
            return res;
        }
    }
}
