using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace GNSS训练
{
    internal class MyAlog
    {
        public List<MyPoint> points=new List<MyPoint>();
        public static double f1=f1*Pow(10,6);
        public static double f2=f2 * Pow(10, 6);
        public static double f3=f3 * Pow(10, 6);
        public static double c;
        public static double lambda1;
        public static double lambda2;
        public static double lambda3;
        public string PrintBG()
        {
            lambda1 = c / f1;
            lambda2 = c / f2;
            lambda3 = c / f3;
            FindOneZTandRecover();
            FindThreeZTandRecover();
            FindTwoZTandRecover();
            Cal_GH();
            Cal_Route();
            StringBuilder read=new StringBuilder();
            read.AppendLine("-------------------单频周跳-------------------");
            read.AppendLine("时间\t\t卫星\t\t是否发生周跳\t\t是否修复\t\t修复前后对比\t\tphi值");
            foreach (var one in points)
            {
                read.AppendLine($"{one.Time,-6}厉元下\t:{one.SatName,-5}\t是否发生周跳:{one.OneZT,-8}\t是否修复:{one.OneRecouver,-8}\t修复前后对比:{one.phireover1,-12}\t{one.phi1}");
            }

            read.AppendLine("-------------------双频周跳-------------------");
            read.AppendLine("时间\t\t卫星\t\t是否发生周跳");
            foreach (var one in points)
            {
                read.AppendLine($"{one.Time,-6}厉元下\t:{one.SatName,-5}\t是否发生周跳:{one.TwoZT}\t是否修复:{one.TwoRecouver}");
            }

            read.AppendLine("-------------------三频周跳-------------------");
            read.AppendLine("时间\t\t卫星\t\t是否发生周跳");
            foreach (var one in points)
            {
                read.AppendLine($"{one.Time,-6}厉元下\t:{one.SatName,-5}\t是否发生周跳:{one.ThreeZT}");
            }
            read.AppendLine("-------------------相位平滑伪距-------------------");
            foreach(var ite in points)
            {
                read.AppendLine($"{ite.Time:f6}厉元下\t:{ite.SatName:f6}\t相位距:{ite.Ps1}\t\t{ite.Ps2}\t\t{ite.Ps3}\t\t多路径MP1:{ite.MP1}\t\t{ite.MP2}\t\t{ite.MP3}");
            }
            return read.ToString();
        }
        /// <summary>
        /// 寻找单频周跳并恢复
        /// </summary>
        public void FindOneZTandRecover()
        {
            var sort = points.GroupBy(t => t.SatName);
            foreach(var ite in sort)
            {
                var data=ite.OrderBy(t=>int.Parse(t.Time)).ToList();
                for(int i=1;i<data.Count;i++)
                {
                    data[i].phi = data[i].phi1 - data[i - 1].phi1;//
                    
                    if (Abs(data[i].phi) >2)
                    {
                        data[i].OneZT = "发生周跳";
                        double deluta_P = data[i].P1 - data[i - 1].P1;
                        double deluta_N = Round(data[i].phi - deluta_P / lambda1);
                        data[i].phireover1 = data[i].phi1 - deluta_N;
                        data[i].residual = lambda1 * (data[i].phireover1 - data[i - 1].phi1) - deluta_P;//计算残差

                        if (Abs(data[i].phireover1)<0.03)
                        {
                            data[i].OneRecouver = "已经修复";
                        }
                        else
                        {
                            data[i].OneRecouver = "无需恢复";
                        }

                    }
                    else
                    {
                        data[i].OneZT = "未发生周跳";
                    }
                }
            }
            
        }
        /// <summary>
        /// 寻找双频周跳并恢复
        /// </summary>
        public void FindTwoZTandRecover()
        {
            var sort = points.GroupBy(t => t.SatName);
            foreach (var ite in sort)
            {
                var data = ite.OrderBy(t => int.Parse(t.Time)).ToList();
                for (int i = 1; i < data.Count; i++)
                {
                    double left = (f1 * data[i].phi1 - f2 * data[i].phi2) / (f1 - f2);
                    double right = (f1 * data[i].phi1 + f2 * data[i].phi2) / (f1 + f2);
                    double NMv = left - right;
                    double left1 = (f1 * data[i-1].phi1 - f2 * data[i-1].phi2) / (f1 - f2);
                    double right1= (f1 * data[i-1].phi1 + f2 * data[i-1].phi2) / (f1 + f2);
                    double NMv1 = left1 - right1;
                    data[i].NMV = NMv - NMv1;
                    if (Abs(data[i].NMV)>0.5)
                    {
                        data[i].TwoZT = "发生周跳";
                        double deulta_NMV = data[i].NMV - data[i - 1].NMV;
                        double deulta_Nw=Round(deulta_NMV);
                        double deulta_phi1 = data[i].phi1 - data[i-1].phi1;
                        double deulta_phi2 = data[i].phi2 - data[i-1].phi2;
                        double over = f1 * deulta_phi1 - f2 * deulta_phi2 + (f1 + f2) * deulta_Nw;
                        double deulta_N1=Round(over/(f1+f2));
                        double deulta_N2 = deulta_N1 - deulta_Nw;
                        data[i].phireover21 = data[i].phi1 - deulta_N1;
                        data[i].phireover22 = data[i].phi2 - deulta_N2;
                        double deulta_MW = data[i].phireover21 - data[i].phireover22;
                        double deulta_GF = lambda1 * (data[i].phireover21) - lambda2 * (data[i].phireover22);
                        double GF = lambda1 * data[i].phi1 - lambda2 * data[i].phi2;
                        double left2 = (f1 * data[i].phireover21 - f2 * data[i].phireover22) / (f1 - f2);
                        double right2 = (f1 * data[i].P1 + f2 * data[i].P2) / (f1 + f2);
                        data[i].NMV = left2 - right2;
                        if (Abs(deulta_GF - GF) < 0.1 || Abs(data[i].NMV)>0.1)
                        {
                            data[i].TwoRecouver = "已经修复了";
                        }
                        

                    }
                    else
                    {
                        data[i].TwoZT = "未发生周跳";
                    }

                }
            }
        }
        /// <summary>
        /// 寻找三频周跳并恢复
        /// </summary>
        public void FindThreeZTandRecover()
        {

            var sort = points.GroupBy(t => t.SatName);
            foreach (var ite in sort)
            {
                var data = ite.OrderBy(t => int.Parse(t.Time)).ToList();
                for (int i = 1; i < data.Count; i++)
                {
                    double GF1 = data[i-1].phi1-(f1/f2)*data[i-1].phi2;
                    double GF2= data[i-1].phi2-(f2/f3)*data[i-1].phi3;
                    double GF3 = data[i].phi1 - (f1 / f3) * data[i].phi3;
                    double GF1_ = data[i].phi1 - (f1 / f2) * data[i].phi2;
                    double GF2_ = data[i].phi2 - (f2 / f3) * data[i].phi3;
                    double GF3_ = data[i].phi1 - (f1 / f3) * data[i].phi3;
                    data[i].GF1 = GF1 - GF1_;
                    data[i].GF2 = GF2 - GF2_;
                    data[i].GF3 = GF3 - GF3_;
                    if (Abs(data[i].GF1)>0.1|| Abs(data[i].GF2) > 0.1|| Abs(data[i].GF3) > 0.1)
                    {
                        data[i].ThreeZT = "发生周跳";
                    }
                    else
                    {
                        data[i].ThreeZT = "未发生周跳";
                    }

                }
            }
        }
        public void Cal_Route()
        {
            var sort1 = points.GroupBy(t => t.SatName);
            foreach(var ite in sort1)
            {
                var data = ite.OrderBy(t => int.Parse(t.Time)).ToList();
                for(int i=0;i<data.Count; i++)
                {
                    double MP1 = data[i].P1 - (2 * f2 * f2 / (f1 * f1 - f2 * f2)) * lambda1 * data[i].phi1 + ((f1 * f1 + f2 * f2 )/ (f1 * f1 - f2 * f2)) * lambda2 * data[i].phi2;
                    double MP2 = data[i].P2 - (2 * f1 * f1 / (f1 * f1 - f2 * f2)) * lambda2 * data[i].phi2 + ((f1 * f1 + f2 * f2) /(f1 * f1 - f2 * f2)) * lambda1 * data[i].phi1;
                    double MP5 = data[i].P3 - (2 * f1 * f1 / (f1 * f1 - f3 * f3)) * lambda3 * data[i].phi3 + ((f1 * f1 + f3 * f3) / (f1 * f1 - f3 * f3)) * lambda1 * data[i].phi1;
                    data[i].MP1= MP1;
                    data[i].MP2= MP2;
                    data[i].MP3 = MP5;
                }
            }

        }
        public void Cal_GH()
        {
            // 按卫星分组处理
            var satelliteGroups = points.GroupBy(t => t.SatName);
            foreach (var group in satelliteGroups)
            {
                // 按时间排序历元数据
                var sortedData = group.OrderBy(t => int.Parse(t.Time)).ToList();
                if (sortedData.Count == 0) continue;

                // 1. 初始化第一个历元的平滑伪距（仅对第0个历元初始化）
                sortedData[0].Ps1 = sortedData[0].P1;
                sortedData[0].Ps2 = sortedData[0].P2;
                sortedData[0].Ps3 = sortedData[0].P3;

                // 平滑窗口计数器（初始为1，随历元递增，最大100）
                double n = 1;

                for (int i = 1; i < sortedData.Count; i++)
                {
                    // 当前历元和上一个历元的数据
                    var current = sortedData[i];
                    var previous = sortedData[i - 1];

                    // 3. 检测周跳（根据三频周跳结果判断，与David逻辑一致）
                    if (current.ThreeZT == "发生周跳")
                    {
                        // 周跳时重置：平滑窗口重置为1，平滑伪距重置为当前原始伪距
                        n = 1;
                        current.Ps1 = current.P1;
                        current.Ps2 = current.P2;
                        current.Ps3 = current.P3;
                    }
                    else
                    {
                        // 无周跳时：窗口递增（最大100）
                        n = Math.Min(n + 1, 100);

                        // 4. 相位平滑伪距公式（修正原公式错误）
                        // 公式：Ps = (当前P / n) + ((n-1)/n) * (上一历元Ps + 波长*(当前相位 - 上一历元相位))
                        current.Ps1 = (current.P1 / n) + ((n - 1) / n) *
                            (previous.Ps1 + lambda1 * (current.phireover1 - previous.phireover1));

                        current.Ps2 = (current.P2 / n) + ((n - 1) / n) *
                            (previous.Ps2 + lambda2 * (current.phireover21 - previous.phireover21));

                        current.Ps3 = (current.P3 / n) + ((n - 1) / n) *
                            (previous.Ps3 + lambda3 * (current.phireover3 - previous.phireover3));
                    }
                }
            }
        }

    }
}
