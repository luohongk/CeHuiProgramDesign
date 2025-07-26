using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _10Trop_NEIL
{
    internal class Algo
    {
        public List<Epoch> w_p_avg = new List<Epoch>();//只是用于存储湿分量投影函数系数表（内插值直接存进 DataCenter 中的 epoches）
        public List<Epoch> d_p_avg = new List<Epoch>();//干分量投影函数系数表 1
        public List<Epoch> d_p_amp = new List<Epoch>();//干分量投影函数系数表 2

        //湿分量投影函数计算
        public void Cal_mwE(DataCenter data)  
        {
            //构建湿分量投影函数系数表
            Epoch e15 = new Epoch(); e15.B = 15.0; e15.aw_avg = 0.00058021897; e15.bw_avg = 0.0014275268; e15.cw_avg = 0.043472961; w_p_avg.Add(e15);
            Epoch e30 = new Epoch(); e30.B = 30.0; e30.aw_avg = 0.00056794847; e30.bw_avg = 0.0015138625; e30.cw_avg = 0.046729510; w_p_avg.Add(e30);
            Epoch e45 = new Epoch(); e45.B = 45.0; e45.aw_avg = 0.00058118019; e45.bw_avg = 0.0014572752; e45.cw_avg = 0.043908931; w_p_avg.Add(e45);
            Epoch e60 = new Epoch(); e60.B = 60.0; e60.aw_avg = 0.00059727542; e60.bw_avg = 0.0015007428; e60.cw_avg = 0.044626982; w_p_avg.Add(e60);
            Epoch e75 = new Epoch(); e75.B = 75.0; e75.aw_avg = 0.00061641693; e75.bw_avg = 0.0017599082; e75.cw_avg = 0.054736038; w_p_avg.Add(e75);

            foreach (Epoch ep in data.epoches)
            {
                Cal_w_p_avg(ep);
                double up = 1 / (1 + (ep.aw_avg / (1 + ep.bw_avg / (1 + ep.cw_avg))));
                double down = 1 / (Sin(ep.E_rad) + (ep.aw_avg / (Sin(ep.E_rad) + ep.bw_avg / (Sin(ep.E_rad) + ep.cw_avg))));
                ep.mwE = up * down;//与书上公式差了一个倒数，结果才对得上
            }                                    
        }
        //湿分量投影函数系数计算
        public void Cal_w_p_avg(Epoch ep)
        {
            //进行内插
            if (Abs(ep.B) <= 15.0)
            {
                ep.aw_avg = w_p_avg[0].aw_avg; ep.bw_avg = w_p_avg[0].bw_avg; ep.cw_avg = w_p_avg[0].cw_avg;
            }
            else if (Abs(ep.B) >= 75.0)
            {
                ep.aw_avg = w_p_avg[4].aw_avg; ep.bw_avg = w_p_avg[4].bw_avg; ep.cw_avg = w_p_avg[4].cw_avg;
            }
            else
            {
                for (int i = 0; i < w_p_avg.Count; i++)
                {
                    if (Abs(ep.B) < w_p_avg[i+1].B)
                    {
                        ep.aw_avg = w_p_avg[i].aw_avg + 
                            (w_p_avg[i+1].aw_avg - w_p_avg[i].aw_avg) * (ep.B - w_p_avg[i].B) / (w_p_avg[i+1].B - w_p_avg[i].B);
                        ep.bw_avg = w_p_avg[i].bw_avg +
                            (w_p_avg[i + 1].bw_avg - w_p_avg[i].bw_avg) * (ep.B - w_p_avg[i].B) / (w_p_avg[i + 1].B - w_p_avg[i].B);
                        ep.cw_avg = w_p_avg[i].cw_avg +
                            (w_p_avg[i + 1].cw_avg - w_p_avg[i].cw_avg) * (ep.B - w_p_avg[i].B) / (w_p_avg[i + 1].B - w_p_avg[i].B);

                        break;
                    }
                }
            }
        }

        //干分量投影函数计算及延迟改正计算
        public void Cal_mdE(DataCenter data)
        {
            //构建干分量投影函数系数表 1
            Epoch e15 = new Epoch(); e15.B = 15.0; e15.ad_avg = 0.0012769934; e15.bd_avg = 0.0029153695; e15.cd_avg = 0.062610505; d_p_avg.Add(e15);
            Epoch e30 = new Epoch(); e30.B = 30.0; e30.ad_avg = 0.0012683230; e30.bd_avg = 0.0029152299; e30.cd_avg = 0.062837393; d_p_avg.Add(e30);
            Epoch e45 = new Epoch(); e45.B = 45.0; e45.ad_avg = 0.0012465397; e45.bd_avg = 0.0029288445; e45.cd_avg = 0.063721774; d_p_avg.Add(e45);
            Epoch e60 = new Epoch(); e60.B = 60.0; e60.ad_avg = 0.0012196049; e60.bd_avg = 0.0029022565; e60.cd_avg = 0.063824265; d_p_avg.Add(e60);
            Epoch e75 = new Epoch(); e75.B = 75.0; e75.ad_avg = 0.0012045996; e75.bd_avg = 0.0029024912; e75.cd_avg = 0.064258455; d_p_avg.Add(e75);
            //构建干分量投影函数系数表 2
            Epoch E15 = new Epoch(); E15.B = 15.0; E15.ad_amp = 0.0; E15.bd_amp = 0.0; E15.cd_amp = 0.0; d_p_amp.Add(E15);
            Epoch E30 = new Epoch(); E30.B = 30.0; E30.ad_amp = 0.000012709626; E30.bd_amp = 0.000021414979; E30.cd_amp = 0.000090128400; d_p_amp.Add(E30);
            Epoch E45 = new Epoch(); E45.B = 45.0; E45.ad_amp = 0.000026523662; E45.bd_amp = 0.000030160779; E45.cd_amp = 0.000043497037; d_p_amp.Add(E45);
            Epoch E60 = new Epoch(); E60.B = 60.0; E60.ad_amp = 0.000034000452; E60.bd_amp = 0.000072562722; E60.cd_amp = 0.00084795348; d_p_amp.Add(E60);
            Epoch E75 = new Epoch(); E75.B = 75.0; E75.ad_amp = 0.000041202191; E75.bd_amp = 0.00011723375; E75.cd_amp = 0.0017037206; d_p_amp.Add(E75);

            foreach (Epoch ep in data.epoches)
            {
                Cal_d_p(ep);
                double up1 = 1 / (1 + (ep.ad / (1 + ep.bd / (1 + ep.cd))));
                double down1 = 1 / (Sin(ep.E_rad) + (ep.ad / (Sin(ep.E_rad) + ep.bd / (Sin(ep.E_rad) + ep.cd))));

                double aht = 2.53e-5;double bht = 5.49e-3; double cht = 1.14e-3;
                double up2 = 1 / (1 + (aht / (1 + bht / (1 + cht))));
                double down2 = 1 / (Sin(ep.E_rad) + (aht / (Sin(ep.E_rad) + bht / (Sin(ep.E_rad) + cht))));

                ep.mdE = up1 * down1 + (1 / Sin(ep.E_rad) - up2 *  down2) * ep.O_H / 1000;
                //延迟改正计算
                ep.delta_S = ep.ZHD * ep.mdE + ep.ZWD * ep.mwE;
            }
        }
        //干分量投影函数系数计算
        public void Cal_d_p(Epoch ep)
        {
            int t0 = 28;

            //进行内插 1 ,计算 ad_avg, bd_avg, ad_avg
            if (Abs(ep.B) <= 15.0)
            {
                ep.ad_avg = d_p_avg[0].ad_avg; 
                ep.bd_avg = d_p_avg[0].bd_avg; 
                ep.cd_avg = d_p_avg[0].cd_avg;
            }
            else if (Abs(ep.B) >= 75.0)
            {
                ep.ad_avg = d_p_avg[4].ad_avg; 
                ep.bd_avg = d_p_avg[4].bd_avg; 
                ep.cd_avg = d_p_avg[4].cd_avg;
            }
            else
            {
                for (int i = 0; i < d_p_avg.Count; i++)
                {
                    if (Abs(ep.B) < d_p_avg[i + 1].B)
                    {
                        ep.ad_avg = d_p_avg[i].ad_avg +
                            (d_p_avg[i + 1].ad_avg - d_p_avg[i].ad_avg) * (ep.B - d_p_avg[i].B) / (d_p_avg[i + 1].B - d_p_avg[i].B);
                        ep.bd_avg = d_p_avg[i].bd_avg +
                            (d_p_avg[i + 1].bd_avg - d_p_avg[i].bd_avg) * (ep.B - d_p_avg[i].B) / (d_p_avg[i + 1].B - d_p_avg[i].B);
                        ep.cd_avg = d_p_avg[i].cd_avg +
                            (d_p_avg[i + 1].cd_avg - d_p_avg[i].cd_avg) * (ep.B - d_p_avg[i].B) / (d_p_avg[i + 1].B - d_p_avg[i].B);

                        break;
                    }
                }
            }
            //进行内插 2 ,计算 ad_amp, bd_amp, cd_amp
            if (Abs(ep.B) <= 15.0)
            {
                ep.ad_amp = d_p_amp[0].ad_amp * Cos(2 * PI *(ep.DOY - t0) / 365.25); 
                ep.bd_amp = d_p_amp[0].bd_amp * Cos(2 * PI * (ep.DOY - t0) / 365.25); 
                ep.cd_amp = d_p_amp[0].cd_amp * Cos(2 * PI * (ep.DOY - t0) / 365.25);
            }
            else if (Abs(ep.B) >= 75.0)
            {
                ep.ad_amp = d_p_amp[4].ad_amp * Cos(2 * PI * (ep.DOY - t0) / 365.25); 
                ep.bd_amp = d_p_amp[4].bd_amp * Cos(2 * PI * (ep.DOY - t0) / 365.25); 
                ep.cd_amp = d_p_amp[4].cd_amp * Cos(2 * PI * (ep.DOY - t0) / 365.25);
            }
            else
            {
                for (int i = 0; i < d_p_amp.Count; i++)
                {
                    if (Abs(ep.B) < d_p_amp[i + 1].B)
                    {
                        ep.ad_amp = (d_p_amp[i].ad_amp + (d_p_amp[i + 1].ad_amp - d_p_amp[i].ad_amp) 
                            * (ep.B - d_p_amp[i].B) / (d_p_amp[i + 1].B - d_p_amp[i].B)) * Cos(2 * PI * (ep.DOY - t0) / 365.25);//
                        ep.bd_amp = (d_p_amp[i].bd_amp + (d_p_amp[i + 1].bd_amp - d_p_amp[i].bd_amp) 
                            * (ep.B - d_p_amp[i].B) / (d_p_amp[i + 1].B - d_p_amp[i].B)) * Cos(2 * PI * (ep.DOY - t0) / 365.25);//
                        ep.cd_amp = (d_p_amp[i].cd_amp + (d_p_amp[i + 1].cd_amp - d_p_amp[i].cd_amp) 
                            * (ep.B - d_p_amp[i].B) / (d_p_amp[i + 1].B - d_p_amp[i].B)) * Cos(2 * PI * (ep.DOY - t0) / 365.25);//

                        break;
                    }
                }
            }
            //两部分相加得到 ad, bd, cd
            ep.ad = ep.ad_avg + ep.ad_amp;
            ep.bd = ep.bd_avg + ep.bd_amp;
            ep.cd = ep.cd_avg + ep.cd_amp;
        }
    }
}
